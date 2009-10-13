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

    public partial class WidgetInstance
    {
        #region Methods

        public WidgetInstance Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;

            this._WidgetZone = default(EntityRef<WidgetZone>);
            this._Widget = default(EntityRef<Widget>);

            return this;
        }

        #endregion Methods
    }
}