namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;

    public partial class aspnet_User
    {
        #region Methods

        public void Detach()
        {
            this.PropertyChanged = null;
            this.PropertyChanging = null;

            this._Pages = new EntitySet<Page>(new Action<Page>(this.attach_Pages), new Action<Page>(this.detach_Pages));
            this._UserSetting = default(EntityRef<UserSetting>);
            this._Tokens = new EntitySet<Token>(new Action<Token>(this.attach_Tokens), new Action<Token>(this.detach_Tokens));
            this._aspnet_UsersInRoles = new EntitySet<aspnet_UsersInRole>(new Action<aspnet_UsersInRole>(this.attach_aspnet_UsersInRoles), new Action<aspnet_UsersInRole>(this.detach_aspnet_UsersInRoles));
            this._RoleTemplates = new EntitySet<RoleTemplate>(new Action<RoleTemplate>(this.attach_RoleTemplates), new Action<RoleTemplate>(this.detach_RoleTemplates));
        }

        #endregion Methods
    }
}