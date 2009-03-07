#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;

    public partial class Page
    {
        #region Properties

        public string TabName
        {
            get
            {
                return this.Title.Replace(' ', '_');
            }
        }

        #endregion Properties

        #region Methods

        public void Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;

            this._Columns = new EntitySet<Column>(new Action<Column>(this.attach_Columns), new Action<Column>(this.detach_Columns));
            this._aspnet_User = default(EntityRef<aspnet_User>);
        }

        #endregion Methods
    }
}