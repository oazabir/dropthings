using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Assert.IsTrue(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.IsTrue(result, "Assert.Retry did not retry the function after exception was thrown");
        }

        [TestMethod]
        public void TestRetryWithDuration()
        {
            bool result = false;
            DateTime firstCallAt = DateTime.Now;
            DateTime secondCallAt = DateTime.Now;
            bool exceptionThrown = false;

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

            Assert.IsTrue(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.IsTrue(result, "Assert.Retry did not retry the function after exception was thrown");
            Assert.IsTrue((secondCallAt - firstCallAt).TotalSeconds > 4.9d, "Assert.Retry took shorter than expected");
            Assert.IsTrue((secondCallAt - firstCallAt).TotalSeconds < 5.1d, "Assert.Retry took longer than expected");
        }

        [TestMethod]
        public void TestRetryWithDurationExceptionHandlerAndFinallyFailing()
        {
            DateTime firstCallAt = DateTime.Now;
            DateTime secondCallAt = DateTime.Now;
            bool exceptionThrown = false;
            bool firstRetry = false;
            bool secondRetry = false;
            bool expectedExceptionFound = false;
            bool allRetryFailed = false;

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

            Assert.IsTrue(exceptionThrown, "Assert.Retry did not invoke the function at all");
            Assert.IsTrue(firstRetry, "Assert.Retry did not retry the function after exception was thrown");
            Assert.IsTrue(secondRetry, "Assert.Retry did not retry the function second time after exception was thrown");
            Assert.IsTrue((secondCallAt - firstCallAt).TotalSeconds > 4.9, "Assert.Retry took shorter than expected");
            Assert.IsTrue((secondCallAt - firstCallAt).TotalSeconds < 5.1, "Assert.Retry took longer than expected");
            Assert.IsTrue(allRetryFailed, "Assert.Retry did not call the final fail handler");
        }

        [TestMethod]
        public void TestDelay()
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            AspectF.Define.Delay(5000).Do(() => { end = DateTime.Now; });

            TimeSpan delay = end - start;
            Assert.IsTrue(delay.TotalSeconds > 4.9d, "Assert.Delay fired too soon {0}", delay.TotalSeconds);
            Assert.IsTrue(delay.TotalSeconds < 5.1d, "Assert.Delay fired too late {0}", delay.TotalSeconds);
        }

        [TestMethod]
        public void TestMustBeNonNullWithValidParameters()
        {
            bool result = false;
            AspectF.Define
                .MustBeNonNull(1, DateTime.Now, string.Empty, "Hello", new object())
                .Do(() =>
                {
                    result = true;
                });
            Assert.IsTrue(result, "Assert.MustBeNonNull did not call the function although all parameters were non-null");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMustBeNonNullWithInvalidParameters()
        {
            bool result = false;
            AspectF.Define
                .MustBeNonNull(1, DateTime.Now, string.Empty, null, "Hello", new object())
                .Do(() =>
            {
                result = true;
            });
            Assert.IsTrue(result, "Assert.MustBeNonNull must not call the function when there's a null parameter");
        }

        [TestMethod]
        public void TestUntil()
        {
            int counter = 10;
            bool callbackFired = false;

            AspectF.Define
                .Until(() =>
                {
                    counter--;
                    if (counter < 0)
                        Assert.Fail("Assert.Until not stopping execution even after test has returned true");

                    return counter == 0;
                })
                .Do(() =>
                {
                    callbackFired = true;
                    Assert.AreEqual(0, counter, "Assert.Until must fire when counter is zero");
                });

            Assert.IsTrue(callbackFired, "Assert.Until never fired the callback");
        }

        [TestMethod]
        public void TestWhile()
        {
            int counter = 10;
            bool callbackFired = false;

            AspectF.Define
                .While(() =>
                {
                    counter--;
                    if (counter < 0)
                        Assert.Fail("Assert.While not stopping execution even after test has returned false");

                    return counter > 0;
                })
                .Do(() =>
                {
                    callbackFired = true;
                    Assert.AreNotSame(0, counter, "Assert.While must fire when counter is zero");
                });

            Assert.IsTrue(callbackFired, "Assert.While never fired the callback");
        }

        [TestMethod]
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

            Assert.IsTrue(callbackFired, "Assert.WhenTrue did not fire callback although all conditions were true");

            bool callbackFired2 = false;
            AspectF.Define.WhenTrue(
                () => 1 == 0, // fail
                () => null == null,
                () => 1 > 0)
                .Do(() => 
                    {
                        callbackFired2 = true;
                    });

            Assert.IsFalse(callbackFired2, "Assert.WhenTrue did not fire callback although all conditions were true");
        }

        [TestMethod]
        public void TestLog()
        {
            StringBuilder buffer = new StringBuilder();
            StringWriter writer = new StringWriter(buffer);

            // Attempt 1: Test one time logging
            AspectF.Define.Log(writer, "Test Log 1").Do(AspectExtensions.DoNothing);

            string logOutput = buffer.ToString();
            Assert.IsTrue(logOutput.Contains("Test Log 1"), "Assert.Log did not produce the right log message");

            DateTime dateOutputResult;
            Assert.IsTrue(DateTime.TryParse(logOutput.Substring(0, logOutput.IndexOf('\t')), out dateOutputResult),
                "Assert.Log did not produce date time in the beginning of log");

            // Attempt 2: Test before and after logging
            buffer.Length = 0;
            AspectF.Define.Log(writer, "Before Log", "After Log")
                .Do(AspectExtensions.DoNothing);

            int expectedContentLength =
                DateTime.Now.ToUniversalTime().ToString().Length + "\t".Length + "Before Log".Length + Environment.NewLine.Length +
                DateTime.Now.ToUniversalTime().ToString().Length + "\t".Length + "After Log".Length + Environment.NewLine.Length;
            Assert.AreEqual(expectedContentLength, buffer.Length,
                "Assert.Log did not write log as expected: {0}-{1}. Expected {1}", buffer.Length, buffer.ToString(), expectedContentLength);

        }

        [TestMethod]
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

            Assert.IsTrue(exceptionThrown, "Aspect.Retry did not call the function at all");
            Assert.IsTrue(retried, "Aspect.Retry did not retry when exception was thrown first time");
            Assert.IsTrue(buffer.ToString().EndsWith("TestRetryAndLog" + Environment.NewLine), "Aspect.Log did not log as expected. {0}", buffer.ToString());
            
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

        [TestMethod]
        public void TestAspectReturn()
        {
            int result = AspectF.Define.Return<int>(() =>
                {
                    return 1;
                });

            Assert.AreEqual(1, result, "Aspect.Return did not return the right value");
        }

        [TestMethod]
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

            Assert.AreEqual(1, result, "Aspect.Return did not return the right value");            
        }

        [TestMethod]
        public void TestAspectAsync()
        {
            bool callExecutedImmediately = false;
            bool callbackFired = false;
            AspectF.Define.RunAsync().Do(() =>
                {
                    callbackFired = true;
                    Assert.IsTrue(callExecutedImmediately, "Aspect.RunAsync Call did not execute asynchronously");
                });
            callExecutedImmediately = true;

            // wait until the async function completes
            while (!callbackFired) Thread.Sleep(100);

            bool callCompleted = false;
            bool callReturnedImmediately = false;
            AspectF.Define.RunAsync(() => Assert.IsTrue(callCompleted, "Aspect.RunAsync Callback did not fire after the call has completed properly"))
                .Do(() =>
                    {
                        callCompleted = true;
                        Assert.IsTrue(callReturnedImmediately, "Aspect.RunAsync call did not run asynchronously");
                    });
            callReturnedImmediately = true;

            while (!callCompleted) Thread.Sleep(100);
        }
    }


}
