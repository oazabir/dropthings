using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Dropthings.RestApi
{
    internal class CustomStateBase
    {
        private AsyncCallback OriginalCallback;
        private object originalState;
        public CustomStateBase(AsyncCallback originalCallback, object originalState)
        {
            this.OriginalCallback = originalCallback;
            this.originalState = originalState;
        }
        public AsyncCallback UserCallback
        {
            get { return this.OriginalCallback; }
        }
        public object UserState
        {
            get { return this.originalState; }
        }
    }
    internal class CustomAsyncResult<T> : IAsyncResult where T : CustomStateBase
    {
        private T CustomData;
        private IAsyncResult OriginalResult;
        public CustomAsyncResult(IAsyncResult originalResult, T customData)
        {
            this.OriginalResult = originalResult;
            this.CustomData = customData;
        }
        public IAsyncResult Inner
        {
            get { return this.OriginalResult; }
        }
        public T AdditionalData
        {
            get { return this.CustomData; }
        }
        #region IAsyncResult Members
        public object AsyncState
        {
            get { return this.CustomData.UserState; }
        }
        public WaitHandle AsyncWaitHandle
        {
            get { return OriginalResult.AsyncWaitHandle; }
        }
        public bool CompletedSynchronously
        {
            get { return OriginalResult.CompletedSynchronously; }
        }
        public bool IsCompleted
        {
            get { return OriginalResult.IsCompleted; }
        }
        #endregion
    }
}
