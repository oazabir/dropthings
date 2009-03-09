using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace Dropthings.DataAccess
{
    public partial class Column
    {
        public void Detach()
        {
            this._Page = default(EntityRef<Page>);
            this._WidgetZone = default(EntityRef<WidgetZone>);			
        }
    }
}
