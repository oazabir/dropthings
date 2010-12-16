using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Dropthings.RestApi
{
    internal class CompletedAsyncResult<T> : IAsyncResult
    {
        private T CustomData;
        private object OriginalState;

        public CompletedAsyncResult(T data, object originalState)
        { 
            this.CustomData = data;
            this.OriginalState = originalState;
        }

        public T Data
        { 
            get 
            { 
                return CustomData; 
            } 
        }

        #region IAsyncResult Members
        public object AsyncState
        { 
            get 
            { 
                return this.OriginalState; 
            } 
        }

        public WaitHandle AsyncWaitHandle
        { 
            get 
            { 
                throw new Exception("The method or operation is not implemented."); 
            } 
        }

        public bool CompletedSynchronously
        { 
            get 
            { 
                return true; 
            } 
        }

        public bool IsCompleted
        { 
            get 
            { 
                return true; 
            } 
        }
        #endregion
    }
}
