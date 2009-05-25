namespace Dropthings.Business.Facade.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.SessionState;

    using Dropthings.Util;

    public class SessionWrapper
    {
        #region Fields

        private const string SESSION_KEY = "Common.Context.AppContext.SessionID";

        private Dictionary<string, object> _LocalStore;
        private bool _Offline = false;
        private WeakReference<HttpSessionState> _Session;

        #endregion Fields

        #region Constructors

        public SessionWrapper()
        {
            this._LocalStore = new Dictionary<string, object>();
            this._Offline = true;
            this._LocalStore.Add(SESSION_KEY, Guid.NewGuid().ToString());
        }

        public SessionWrapper(HttpSessionState session)
        {
            this._Session = new WeakReference<HttpSessionState>(session);
            this._Offline = false;
        }

        #endregion Constructors

        #region Properties

        public int Count
        {
            get
            {
                if (this._Offline)
                    return this._LocalStore.Count;
                else
                    return this._Session.Target.Count;
            }
        }

        public string SessionID
        {
            get
            {
                if (this._Offline)
                    return this._LocalStore[SESSION_KEY] as string;
                else
                    return this._Session.Target.SessionID;
            }
        }

        #endregion Properties

        #region Indexers

        public object this[string name]
        {
            get
            {
                if (this._Offline)
                    return this._LocalStore[name];
                else
                    return this._Session.Target[name];
            }
            set
            {
                if (this._Offline)
                    this._LocalStore[name] = value;
                else
                    this._Session.Target[name] = value;
            }
        }

        #endregion Indexers

        #region Methods

        public void Abandon()
        {
            if (this._Offline)
                this._LocalStore.Clear();
            else
                this._Session.Target.Clear();
        }

        #endregion Methods
    }
}