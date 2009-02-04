#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace AJAXASMXHandler
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class AsyncWebMethodState : IDisposable
    {
        #region Fields

        public HttpContext Context;

        internal object ExtraData;
        internal WebMethodDef MethodDef;
        internal string MethodName;
        internal WebServiceDef ServiceDef;
        internal IDisposable Target;

        #endregion Fields

        #region Constructors

        public AsyncWebMethodState(object s)
            : this((AsyncWebMethodState)s)
        {
        }

        public AsyncWebMethodState(AsyncWebMethodState s)
            : this(s.MethodName, s.Target, s.ServiceDef, s.MethodDef, s.Context, s.ExtraData)
        {
        }

        internal AsyncWebMethodState(string methodName,
            IDisposable target, WebServiceDef wsDef, WebMethodDef wmDef, HttpContext context,
            object extraData)
        {
            this.MethodName = methodName;
            this.Target = target;
            this.ServiceDef = wsDef;
            this.MethodDef = wmDef;
            this.Context = context;
            this.ExtraData = extraData;
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            this.MethodName = null;
            this.Target = null;
            this.ServiceDef = null;
            this.MethodDef = null;
            this.Context = null;
            this.ExtraData = null;
        }

        #endregion Methods
    }
}