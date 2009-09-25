using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Dropthings.Business;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;

public partial class Setup : System.Web.UI.Page
{
    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        SetupDefaultSetting();
    }

    private static void SetupDefaultSetting()
    {
        //setup default roles, template user and role template
        using (var facade = new Facade(new AppContext(string.Empty, string.Empty)))
        {
            facade.SetupDefaultSetting();
        }
    }

    #endregion Methods
}