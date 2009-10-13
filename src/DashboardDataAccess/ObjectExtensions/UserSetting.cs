namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;

    public partial class UserSetting
    {
        #region Methods

        public UserSetting Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;

            this._aspnet_User = default(EntityRef<aspnet_User>);

            return this;
        }

        #endregion Methods
    }
}