using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Util;
using OmarALZabir.AspectF;

public partial class Tools_Flush : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Services.Get<ICache>().Flush();
    }
}
