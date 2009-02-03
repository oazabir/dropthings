#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TimedLog : IDisposable
    {
        #region Fields

        private string _Message;
        private long _StartTicks;

        #endregion Fields

        #region Constructors

        public TimedLog(string userName, string message)
        {
            this._Message = userName + '\t' + message;
            this._StartTicks = DateTime.Now.Ticks;
        }

        #endregion Constructors

        #region Methods

        void IDisposable.Dispose()
        {
            string msg = this._Message + '\t' + TimeSpan.FromTicks(DateTime.Now.Ticks - this._StartTicks).TotalSeconds.ToString();
            //EntLibHelper.PerformanceLog(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        #endregion Methods
    }
}