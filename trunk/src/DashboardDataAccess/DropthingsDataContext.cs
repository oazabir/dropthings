namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public partial class DropthingsDataContext
    {
        #region Methods

        public new void Dispose()
        {
            if (base.Connection != null)
                if (base.Connection.State != System.Data.ConnectionState.Closed)
                    base.Connection.Close();

            base.Dispose();
        }

        #endregion Methods
    }
}