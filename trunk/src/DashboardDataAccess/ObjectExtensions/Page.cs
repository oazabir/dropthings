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

        public string UserTabName
        {
            get
            {
                return this.Title.Replace(' ', '_');
            }
        }

        public string LockedTabName
        {
            get
            {
                return this.Title.Replace(' ', '_') + "_Locked";
            }
        }

        #endregion Properties

        #region Methods

        public static int[] GetColumnWidths(int layoutType)
        {
            int[] columnWidths;
            if (layoutType == 2)
                columnWidths = new int[] { 25, 75 };
            else if (layoutType == 3)
                columnWidths = new int[] { 75, 25 };
            else if (layoutType == 4)
                columnWidths = new int[] { 100 };
            else
                columnWidths = new int[] { 33, 33, 33 };

            return columnWidths;
        }

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