using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.Util;
using System.IO;
using System.Threading;
using Xunit;

namespace Dropthings.Test.UnitTests.Dropthings.Util
{
    public class AspectTest
    {
        [Fact]
        public void TestRetry()
        {
            bool result = false;
            bool exceptionThrown = false;

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry().Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            exceptionThrown = true;
                            throw new ApplicationException("Test exception");
                        }
                        else
                        {
                            result = true;
                        }
                    });
                });

            Assert.True(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.True(result, "Assert.Retry did not retry the function after exception was thrown");
        }

        [Fact]
        public void TestRetryWithDuration()
        {
            bool result = false;
            DateTime firstCallAt = DateTime.Now;
            DateTime secondCallAt = DateTime.Now;
            bool exceptionThrown = false;

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry(5000).Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            firstCallAt = DateTime.Now;
                            exceptionThrown = true;
                            throw new ApplicationException("Test exception");
                        }
                        else
                        {
                            secondCallAt = DateTime.Now;
                            result = true;
                        }
                    });
                });

            Assert.True(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.True(result, "Assert.Retry did not retry the function after exception was thrown");
            Assert.InRange<Double>((secondCallAt - firstCallAt).TotalSeconds, 4.9d, 5.1d);
        }

        [Fact]
        public void TestRetryWithDurationExceptionHandlerAndFinallyFailing()
        {
            DateTime firstCallAt = DateTime.Now;
            DateTime secondCallAt = DateTime.Now;
            bool exceptionThrown = false;
            bool firstRetry = false;
            bool secondRetry = false;
            bool expectedExceptionFound = false;
            bool allRetryFailed = false;

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry(5000, 2,
                        x => { expectedExceptionFound = x is ApplicationException; },
                        () => { allRetryFailed = true; })
                        .Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            firstCallAt = DateTime.Now;
                            exceptionThrown = true;
                            throw new ApplicationException("First exception");
                        }
                        else if (!firstRetry)
                        {
                            secondCallAt = DateTime.Now;
                            firstRetry = true;
                            throw new ApplicationException("Second exception");
                        }
                        else if (!secondRetry)
                        {
                            secondRetry = true;
                            throw new ApplicationException("Third exception");
                        }
                    });
                });

            Assert.True(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.True(firstRetry, "Assert.Retry did not retry the function after exception was thrown");
            Assert.True(secondRetry, "Assert.Retry did not retry the function second time after exception was thrown");
            Assert.InRange<Double>((secondCallAt - firstCallAt).TotalSeconds, 4.9d, 5.1d); 
            Assert.True(allRetryFailed, "Assert.Retry did not call the final fail handler");
        }

        [Fact]
        public void TestDelay()
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            AspectF.Define.Delay(5000).Do(() => { end = DateTime.Now; });

            TimeSpan delay = end - start;
            Assert.InRange<double>(delay.TotalSeconds, 4.9d, 5.1d);
        }

        [Fact]
        public void TestMustBeNonNullWithValidParameters()
        {
            bool result = false;

            Assert.DoesNotThrow(delegate
            {
                AspectF.Define
                .MustBeNonNull(1, DateTime.Now, string.Empty, "Hello", new object())
                .Do(delegate
                {
                    result = true;
                });
            });

            Assert.True(result, "Assert.MustBeNonNull did not call the function although all parameters were non-null");
        }

        [Fact]
        public void TestMustBeNonNullWithInvalidParameters()
        {
            bool result = false;

            Assert.Throws(typeof(ArgumentException), delegate
            {
                AspectF.Define
                    .MustBeNonNull(1, DateTime.Now, string.Empty, null, "Hello", new object())
                    .Do(() =>
                {
                    result = true;
                });

                Assert.True(result, "Assert.MustBeNonNull must not call the function when there's a null parameter");
            });
            
        }

        [Fact]
        public void TestUntil()
        {
            int counter = 10;
            bool callbackFired = false;

            AspectF.Define
                .Until(() =>
                {
                    counter--;
                    
                    Assert.InRange<int>(counter, 0, 9);

                    return counter == 0;
                })
                .Do(() =>
                {
                    callbackFired = true;
                    Assert.Equal<int>(0, counter);
                });

            Assert.True(callbackFired, "Assert.Until never fired the callback");
        }

        [Fact]
        public void TestWhile()
        {
            int counter = 10;
            bool callbackFired = false;

            AspectF.Define
                .While(() =>
                {
                    counter--;
                    
                    Assert.InRange(counter, 0, 9);

                    return counter > 0;
                })
                .Do(() =>
                {
                    callbackFired = true;
                    Assert.Equal<int>(0, counter);
                });

            Assert.True(callbackFired, "Assert.While never fired the callback");
        }

        [Fact]
        public void TestWhenTrue()
        {
            bool callbackFired = false;
            AspectF.Define.WhenTrue(
                () => 1 == 1,
                () => null == null,
                () => 1 > 0)
                .Do(() => 
                    {
                        callbackFired = true;
                    });

            Assert.True(callbackFired, "Assert.WhenTrue did not fire callback although all conditions were true");

            bool callbackFired2 = false;
            AspectF.Define.WhenTrue(
                () => 1 == 0, // fail
                () => null == null,
                () => 1 > 0)
                .Do(() => 
                    {
                        callbackFired2 = true;
                    });

            Assert.False(callbackFired2, "Assert.WhenTrue did not fire callback although all conditions were true");
        }

        [Fact]
        public void TestLog()
        {
            StringBuilder buffer = new StringBuilder();
            StringWriter writer = new StringWriter(buffer);

            // Attempt 1: Test one time logging
            AspectF.Define.Log(writer, "Test Log 1").Do(AspectExtensions.DoNothing);

            string logOutput = buffer.ToString();
            Assert.True(logOutput.Contains("Test Log 1"), "Assert.Log did not produce the right log message");

            DateTime dateOutputResult;
            Assert.True(DateTime.TryParse(logOutput.Substring(0, logOutput.IndexOf('\t')), out dateOutputResult),
                "Assert.Log did not produce date time in the beginning of log");

            // Attempt 2: Test before and after logging
            buffer.Length = 0;
            AspectF.Define.Log(writer, "Before Log", "After Log")
                .Do(AspectExtensions.DoNothing);

            int expectedContentLength =
                DateTime.Now.ToUniversalTime().ToString().Length + "\t".Length + "Before Log".Length + Environment.NewLine.Length +
                DateTime.Now.ToUniversalTime().ToString().Length + "\t".Length + "After Log".Length + Environment.NewLine.Length;
            Assert.Equal(expectedContentLength, buffer.Length);                
        }

        [Fact]
        public void TestRetryAndLog()
        {
            StringBuilder buffer = new StringBuilder();
            StringWriter writer = new StringWriter(buffer);

            // Attempt 1: Test log and Retry together
            bool exceptionThrown = false;
            bool retried = false;
            AspectF.Define.Log(writer, "TestRetryAndLog").Retry()
                .Do(() => 
            {
                if (!exceptionThrown)
                {
                    exceptionThrown = true;
                    throw new ApplicationException("First exception thrown which should be ignored");
                }
                else
                {
                    retried = true;
                }
            });

            Assert.True(exceptionThrown, "Aspect.Retry did not call the function at all");
            Assert.True(retried, "Aspect.Retry did not retry when exception was thrown first time");
            Assert.True(buffer.ToString().EndsWith("TestRetryAndLog" + Environment.NewLine));
            
            // Attempt 2: Test Log Before and After with Retry together            
            AspectF.Define.Log(writer, "BeforeLog", "AfterLog").Retry()
                .Do(() =>
                {
                    if (!exceptionThrown)
                    {
                        exceptionThrown = true;
                        throw new ApplicationException("First exception thrown which should be ignored");
                    }
                    else
                    {
                        retried = true;
                    }
                });

        }

        [Fact]
        public void TestAspectReturn()
        {
            int result = AspectF.Define.Return<int>(() =>
                {
                    return 1;
                });

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestAspectReturnWithOtherAspects()
        {
            StringWriter writer = new StringWriter(new StringBuilder());

            int result = AspectF.Define
                .Log(writer, "Test Logging")
                .Retry(2)
                .MustBeNonNull(1, DateTime.Now, string.Empty)
                .Return<int>(() =>
                {
                    return 1;
                });

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestAspectAsync()
        {
            bool callExecutedImmediately = false;
            bool callbackFired = false;
            AspectF.Define.RunAsync().Do(() =>
                {
                    callbackFired = true;
                    Assert.True(callExecutedImmediately, "Aspect.RunAsync Call did not execute asynchronously");
                });
            callExecutedImmediately = true;

            // wait until the async function completes
            while (!callbackFired) Thread.Sleep(100);

            bool callCompleted = false;
            bool callReturnedImmediately = false;
            AspectF.Define.RunAsync(() => Assert.True(callCompleted, "Aspect.RunAsync Callback did not fire after the call has completed properly"))
                .Do(() =>
                    {
                        callCompleted = true;
                        Assert.True(callReturnedImmediately, "Aspect.RunAsync call did not run asynchronously");
                    });
            callReturnedImmediately = true;

            while (!callCompleted) Thread.Sleep(100);
        }

        [Fact]
        public void TestTrapLog()
        {
            StringBuilder buffer = new StringBuilder();
            StringWriter writer = new StringWriter(buffer);

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.TrapLog(writer).Do(() =>
                        {
                            throw new ApplicationException("Parent Exception",
                                new ApplicationException("Child Exception",
                                    new ApplicationException("Grandchild Exception")));
                        });
                });

            string logOutput = buffer.ToString();

            Assert.True(logOutput.Contains("Parent Exception"));
            Assert.True(logOutput.Contains("Child Exception"));
            Assert.True(logOutput.Contains("Grandchild Exception"));
        }

        [Fact]
        public void TestTrapLogThrow()
        {
            StringBuilder buffer = new StringBuilder();
            StringWriter writer = new StringWriter(buffer);

            Assert.Throws(typeof(ApplicationException), () =>
            {
                AspectF.Define.TrapLogThrow(writer).Do(() =>
                {
                    throw new ApplicationException("Parent Exception",
                        new ApplicationException("Child Exception",
                            new ApplicationException("Grandchild Exception")));
                });
            });

            string logOutput = buffer.ToString();

            Assert.True(logOutput.Contains("Parent Exception"));
            Assert.True(logOutput.Contains("Child Exception"));
            Assert.True(logOutput.Contains("Grandchild Exception"));
        }
    }

    

}
