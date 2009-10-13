namespace Dropthings.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Configuration;

    public class DropthingsDataContext2 : DropthingsDataContext, IDisposable
    {
        #region Constructors

        public DropthingsDataContext2(string connectionString)
            : base(connectionString)
        {
        }

        public DropthingsDataContext2()
            : this(DropthingsDataContext.GetConnectionString())
        {
        }

        #endregion Constructors

        #region Methods

        public new void Dispose()
        {
            if (base.Connection.State != System.Data.ConnectionState.Closed)
            {
                base.Connection.Close();
                base.Connection.Dispose();
            }

            base.Dispose();
        }

        #endregion Methods
    }
}