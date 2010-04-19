namespace Dropthings.Business.Facade.Context
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Dropthings.Util;
    using System.Diagnostics;

    public class AppContext : IDisposable
    {
        #region Fields

        private const string HTTP_CONTEXT_KEY = "Dropthings.Business.Facade.Context.AppContext";

        // Store an instance of context once per thread when run in offline mode
        [ThreadStatic]
        private static AppContext _Context;

        private NameObjectCollectionBase _Application;
        private bool _Disposed = false;
        private IDictionary _Items;
        private SessionWrapper _SessionWrapper;

        #endregion Fields

        #region Constructors

        public AppContext(HttpContext httpContext, string currentUserId, string currentUserName) : this(currentUserId, currentUserName)
        {
            this._SessionWrapper = new SessionWrapper(httpContext.Session);
            this._Items = httpContext.Items;
            this._Application = httpContext.Application;
            httpContext.Items[HTTP_CONTEXT_KEY] = this;
        }

        public AppContext(string currentUserId, string currentUserName)
        {
            _Context = this;
            this._SessionWrapper = new SessionWrapper();
            this._Items = new Dictionary<string, object>();
            this._Application = new NameObjectCollection();
            CurrentUserId = currentUserId;
            CurrentUserName = currentUserName;
        }

        ~AppContext()
        {
            if (!_Disposed)
                this.Dispose();
        }

        #endregion Constructors

        #region Properties

        public static AppContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    // offline mode
                    // There will be only one instance of AppContext per thread
                    return _Context;
                }
                else
                {
                    // Web Mode
                    return GetContext(HttpContext.Current);
                }
            }
        }

        /// <summary>
        /// Get an existing AppContext, already prepared and stored in HttpContext.
        /// Generally Global.asax does this. It prepares the AppContext for the request
        /// and stores in HttpContext so that anyone can get access to it during the 
        /// request pipeline.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static AppContext GetContext(HttpContext httpContext)
        {            
            return httpContext.Items[HTTP_CONTEXT_KEY] as AppContext;         
        }

        public NameObjectCollectionBase Application
        {
            get { return this._Application; }
        }

        public string CurrentUserId
        {
            get; set;
        }

        public string CurrentUserName
        {
            get; set;
        }

        public IDictionary Items
        {
            get { return this._Items; }
        }

        public SessionWrapper Session
        {
            get
            {
                return this._SessionWrapper;
            }
        }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            foreach (object item in this._Items.Values)
            {
                if (item != this)
                    if (item is IDisposable)
                        try
                        {
                            ((IDisposable)item).Dispose();
                        }
                        catch (Exception x)
                        {
                            Debug.WriteLine(x);
                        }
            }

            _Disposed = true;
        }

        #endregion Methods
    }
}