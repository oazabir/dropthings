#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace AJAXASMXHandler
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ETagMethodAttribute : Attribute
    {
        #region Fields

        private bool _Enabled;

        #endregion Fields

        #region Constructors

        public ETagMethodAttribute()
        {
            this._Enabled = true;
        }

        public ETagMethodAttribute(bool enabled)
        {
            this._Enabled = enabled;
        }

        #endregion Constructors

        #region Properties

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        #endregion Properties
    }
}