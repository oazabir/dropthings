using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Util;
using OmarALZabir.AspectF;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AspectF.Define
            .TrapLog(Services.Get<ILogger>())
            .Retry(Services.Get<ILogger>())
            .Do(() =>
            {
                throw new ApplicationException("This is a test exception");
            });

        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            Guid guid1 = facade.GetUserGuidFromUserName(Profile.UserName);
            Guid guid2 = facade.GetUserGuidFromUserName(Profile.UserName);

            if (guid1 != guid2)
                throw new ApplicationException("Cache failed");
        }
    }
}
