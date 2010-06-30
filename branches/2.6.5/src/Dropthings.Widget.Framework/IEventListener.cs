namespace Dropthings.Widget.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IEventListener
    {
        #region Methods

        void AcceptEvent(object sender, EventArgs e);

        #endregion Methods
    }
}