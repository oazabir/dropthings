using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dropthings.Util;
using System.IO;
using System.Threading;
using Xunit;
using Moq;

namespace Dropthings.Test.UnitTests.Dropthings.Util
{
    public class AspectTest
    {
        private ILogger MockLoggerForException(params Exception[] exceptions)
        {
            var logger = new Mock<ILogger>();
            exceptions.Each(x => logger.Expect(l => l.LogException(x)).Verifiable());
            return logger.Object;
        }
        [Fact]
        public void TestRetry()
        {
            bool result = false;
            bool exceptionThrown = false;

            var ex = new ApplicationException("Test exception");
            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry(MockLoggerForException(ex)).Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            exceptionThrown = true;
                            throw ex;
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

            var ex = new ApplicationException("Test exception");
            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry(5000, MockLoggerForException(ex)).Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            firstCallAt = DateTime.Now;
                            exceptionThrown = true;
                            throw ex;
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

            var ex1 = new ApplicationException("First exception");
            var ex2 = new ApplicationException("Second exception");
            var ex3 = new ApplicationException("Third exception");

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.Retry(5000, 2,
                        x => { expectedExceptionFound = x is ApplicationException; },
                        errors => { allRetryFailed = true; },
                        MockLoggerForException(ex1, ex2, ex3))
                        .Do(() =>
                    {
                        if (!exceptionThrown)
                        {
                            firstCallAt = DateTime.Now;
                            exceptionThrown = true;
                            throw ex1;
                        }
                        else if (!firstRetry)
                        {
                            secondCallAt = DateTime.Now;
                            firstRetry = true;
                            throw ex2;
                        }
                        else if (!secondRetry)
                        {
                            secondRetry = true;
                            throw ex3;
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
            var categories = new string[] { "Category1", "Category2" };
            var logger1 = new Mock<ILogger>();
            logger1.Expect(l => l.Log(categories, "Test Log 1")).Verifiable();
            // Attempt 1: Test one time logging
            AspectF.Define
                .Log(logger1.Object, categories, "Test Log 1")
                .Do(AspectExtensions.DoNothing);
            logger1.Verify();

            // Attempt 2: Test before and after logging
            var logger2 = new Mock<ILogger>();
            logger2.Expect(l => l.Log(categories, "Before Log")).Verifiable();
            logger2.Expect(l => l.Log(categories, "After Log")).Verifiable();
            AspectF.Define
                .Log(logger2.Object, categories, "Before Log", "After Log")
                .Do(AspectExtensions.DoNothing);
            logger2.VerifyAll();
        }

        [Fact]
        public void TestRetryAndLog()
        {
            // Attempt 1: Test log and Retry together
            bool exceptionThrown = false;
            bool retried = false;

            var logger = new Mock<ILogger>();
            logger.Expect(l => l.Log("TestRetryAndLog")).Verifiable();

            var ex = new ApplicationException("First exception thrown which should be ignored");
            AspectF.Define
                .Log(logger.Object, "TestRetryAndLog")
                .Retry(MockLoggerForException(ex))
                .Do(() => 
            {
                if (!exceptionThrown)
                {
                    exceptionThrown = true;
                    throw ex;
                }
                else
                {
                    retried = true;
                }
            });
            logger.Verify();

            Assert.True(exceptionThrown, "Aspect.Retry did not call the function at all");
            Assert.True(retried, "Aspect.Retry did not retry when exception was thrown first time");
         
            // Attempt 2: Test Log Before and After with Retry together            
            var logger2 = new Mock<ILogger>();
            logger2.Expect(l => l.Log("BeforeLog")).Verifiable();
            logger2.Expect(l => l.Log("AfterLog")).Verifiable();
            AspectF.Define
                .Log(logger2.Object, "BeforeLog", "AfterLog")
                .Retry(MockLoggerForException(ex))
                .Do(() =>
                {
                    if (!exceptionThrown)
                    {
                        exceptionThrown = true;
                        throw ex;
                    }
                    else
                    {
                        retried = true;
                    }
                });
            logger2.VerifyAll();
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
            var logger = new Mock<ILogger>();
            logger.Expect(l => l.Log("Test Logging")).Verifiable();

            int result = AspectF.Define
                .Log(logger.Object, "Test Logging")
                .Retry(2, new Mock<ILogger>().Object)
                .MustBeNonNull(1, DateTime.Now, string.Empty)
                .Return<int>(() =>
                {
                    return 1;
                });

            logger.VerifyAll();
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
            var exception = new ApplicationException("Parent Exception",
                                new ApplicationException("Child Exception",
                                    new ApplicationException("Grandchild Exception")));
            var logger = new Mock<ILogger>();
            logger.Expect(l => l.LogException(exception)).Verifiable();

            Assert.DoesNotThrow(() =>
                {
                    AspectF.Define.TrapLog(logger.Object).Do(() =>
                        {
                            throw exception;
                        });
                });
            logger.VerifyAll();            
        }

        [Fact]
        public void TestTrapLogThrow()
        {
            var exception = new ApplicationException("Parent Exception",
                                new ApplicationException("Child Exception",
                                    new ApplicationException("Grandchild Exception")));
            var logger = new Mock<ILogger>();
            logger.Expect(l => l.LogException(exception)).Verifiable();

            
            Assert.Throws(typeof(ApplicationException), () =>
            {
                AspectF.Define.TrapLogThrow(logger.Object).Do(() =>
                {
                    throw exception;
                });
            });

            logger.VerifyAll();            
        }
    }

    

}
