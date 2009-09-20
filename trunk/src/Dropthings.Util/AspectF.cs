using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Dropthings.Util
{
    /// <summary>
    /// AspectF
    /// (C) Omar AL Zabir 2009 All rights reserved.
    /// 
    /// AspectF lets you add strongly typed Aspects within you code, 
    /// anywhere in the code, in a fluent way. In common AOP frameworks, 
    /// you define aspects as individual classes and you leave indication 
    /// in the code where the aspect needs to be injected. A weaver 
    /// then weaves it into the code for you. You can also implement AOP
    /// using Attributes and by inheriting your classes from MarshanByRef. 
    /// But that's not an option for you always to do so. There's also 
    /// another way of doing AOP using DynamicProxy.
    /// 
    /// AspectF tries to avoid all these special tricks. It has no need 
    /// for a weaver (or any post build tool). It also does not require
    /// extending classes from MarshalByRef or using DynamicProxy.
    /// 
    /// AspectF offers a plain vanilla way of putting aspects within 
    /// your methods. You can wrap your code using Aspects 
    /// by using standard wellknown C#/VB.NET code. 
    /// </summary>
    public class AspectF
    {
        /// <summary>
        /// Chain of aspects to invoke
        /// </summary>
        public Action<Action> Chain = null;

        /// <summary>
        /// Create a composition of function e.g. f(g(x))
        /// </summary>
        /// <param name="newAspectDelegate">A delegate that offers an aspect's behavior. 
        /// It's added into the aspect chain</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public AspectF Combine(Action<Action> newAspectDelegate)
        {
            if (this.Chain == null)
            {
                this.Chain = newAspectDelegate;
            }
            else
            {
                Action<Action> existingChain = this.Chain;
                Action<Action> callAnother = (work) => existingChain(() => newAspectDelegate(work));
                this.Chain = callAnother;
            }
            return this;
        }

        /// <summary>
        /// Execute your real code applying the aspects over it
        /// </summary>
        /// <param name="work">The actual code that needs to be run</param>
        [DebuggerStepThrough]
        public void Do(Action work)
        {
            if (this.Chain == null)
            {
                work();
            }
            else
            {
                this.Chain(work);
            }
        }

        /// <summary>
        /// Execute your real code applying aspects over it.
        /// </summary>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="work">The actual code that needs to be run</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TReturnType Return<TReturnType>(Func<TReturnType> work)
        {
            TReturnType returnValue = default(TReturnType);
            if (this.Chain == null)
            {
                return work();
            }
            else
            {
                this.Chain(() =>
                {
                    returnValue = work();
                });
            }
            return returnValue;
        }
        
        /// <summary>
        /// Handy property to start writing aspects using fluent style
        /// </summary>
        public static AspectF Define
        {
            [DebuggerStepThrough]
            get
            {
                return new AspectF();
            }
        }
    }
    
    public static class AspectExtensions
    {
        [DebuggerStepThrough]
        public static void DoNothing()
        {
        }

        [DebuggerStepThrough]
        public static void DoNothing(params object[] whatever)
        {
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects)
        {
            return aspects.Combine((work) => 
                Retry(1000, 1, (error) => DoNothing(error), DoNothing, work));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration)
        {
            return aspects.Combine((work) => 
                Retry(retryDuration, 1, (error) => DoNothing(error), DoNothing, work));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration, 
            Action<Exception> errorHandler)
        {
            return aspects.Combine((work) => 
                Retry(retryDuration, 1, errorHandler, DoNothing, work));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration, 
            int retryCount, Action<Exception> errorHandler)
        {
            return aspects.Combine((work) =>
                Retry(retryDuration, retryCount, errorHandler, DoNothing, work));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration,
            int retryCount, Action<Exception> errorHandler, Action retryFailed)
        {
            return aspects.Combine((work) => 
                Retry(retryDuration, retryCount, errorHandler, retryFailed, work));
        }

        [DebuggerStepThrough]
        public static void Retry(int retryDuration, int retryCount, 
            Action<Exception> errorHandler, Action retryFailed, Action work)
        {
            do
            {
                try
                {
                    work();
                }
                catch (Exception x)
                {
                    errorHandler(x);
                    System.Threading.Thread.Sleep(retryDuration);
                }
            } while (retryCount-- > 0);
            retryFailed();
        }

        [DebuggerStepThrough]
        public static AspectF Delay(this AspectF aspect, int milliseconds)
        {
            return aspect.Combine((work) =>
            {
                System.Threading.Thread.Sleep(milliseconds);
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustBeNonDefault<T>(this AspectF aspect, params T[] args)
            where T:IComparable
        {
            return aspect.Combine((work) =>
            {
                T defaultvalue = default(T);
                for (int i = 0; i < args.Length; i++)
                {
                    T arg = args[i];
                    if (arg == null || arg.Equals(defaultvalue))
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustBeNonNull(this AspectF aspect, params object[] args)
        {
            return aspect.Combine((work) =>
            {
                for (int i = 0; i < args.Length; i++)
                {
                    object arg = args[i];
                    if (arg == null)
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Until(this AspectF aspect, Func<bool> test)
        {
            return aspect.Combine((work) =>
            {
                while (!test()) ;
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF While(this AspectF aspect, Func<bool> test)
        {
            return aspect.Combine((work) =>
            {
                while (test()) ;
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF WhenTrue(this AspectF aspect, params Func<bool>[] conditions)
        {
            return aspect.Combine((work) =>
            {
                foreach (Func<bool> condition in conditions)
                    if (!condition())
                        return;

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, TextWriter logWriter, 
            string logMessage, params object[] arg)
        {
            return aspect.Combine((work) =>
            {
                logWriter.Write(DateTime.Now.ToUniversalTime().ToString());
                logWriter.Write('\t');
                logWriter.Write(logMessage, arg);
                logWriter.Write(Environment.NewLine);

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, TextWriter logWriter, 
            string beforeMessage, string afterMessage)
        {
            return aspect.Combine((work) =>
            {
                logWriter.Write(DateTime.Now.ToUniversalTime().ToString());
                logWriter.Write('\t');
                logWriter.Write(beforeMessage);
                logWriter.Write(Environment.NewLine);

                work();

                logWriter.Write(DateTime.Now.ToUniversalTime().ToString());
                logWriter.Write('\t');
                logWriter.Write(afterMessage);
                logWriter.Write(Environment.NewLine);
            });
        }

        [DebuggerStepThrough]
        public static AspectF HowLong(this AspectF aspect, TextWriter logWriter, 
            string startMessage, string endMessage)
        {
            return aspect.Combine((work) =>
            {
                DateTime start = DateTime.Now.ToUniversalTime();
                logWriter.Write(start.ToString());
                logWriter.Write('\t');
                logWriter.Write(startMessage);
                logWriter.Write(Environment.NewLine);

                work();

                DateTime end = DateTime.Now.ToUniversalTime();
                TimeSpan duration = end - start;
                logWriter.Write(end.ToString());
                logWriter.Write('\t');
                logWriter.Write(string.Format(endMessage, duration.TotalMilliseconds, 
                    duration.TotalSeconds, duration.TotalMinutes, duration.TotalHours,
                    duration.TotalDays));
                logWriter.Write(Environment.NewLine);
            });
        }

        [DebuggerStepThrough]
        public static AspectF TrapLog(this AspectF aspect, TextWriter logger)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception x)
                {
                    Exception current = x;
                    int indent = 0;
                    while (current != null)
                    {
                        string message = new string(Enumerable.Repeat('\t', indent).ToArray()) 
                            + current.Message;
                        Debug.WriteLine(message);
                        logger.WriteLine(message);
                        current = current.InnerException;
                        indent++;
                    }
                    Debug.WriteLine(x.StackTrace);
                    logger.WriteLine(x.StackTrace);
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF TrapLogThrow(this AspectF aspect, TextWriter logger)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception x)
                {
                    Exception current = x;
                    int indent = 0;
                    while (current != null)
                    {
                        string message = new string(Enumerable.Repeat('\t', indent).ToArray()) 
                            + current.Message;
                        Debug.WriteLine(message);
                        logger.WriteLine(message);
                        current = current.InnerException;
                        indent++;
                    }
                    Debug.WriteLine(x.StackTrace);
                    logger.WriteLine(x.StackTrace);
                    throw;
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect, Action completeCallback)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult => 
                { 
                    work.EndInvoke(asyncresult); completeCallback(); 
                }, null));
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult => 
                { 
                    work.EndInvoke(asyncresult); 
                }, null));
        }
    }
}
