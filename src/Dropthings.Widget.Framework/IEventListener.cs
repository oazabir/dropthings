using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.Widget.Framework
{
    public interface IEventListener
    {
        void AcceptEvent(object sender, EventArgs e);
    }
}
