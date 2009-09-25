using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Util;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AspectF.Define.Retry(Services.Get<ILogger>()).Do(() =>
            {
                throw new ApplicationException("This is a test exception");
            });
    }
}
