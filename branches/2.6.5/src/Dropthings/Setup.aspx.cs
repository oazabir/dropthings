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
using Dropthings.Util;

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
        var facade = Services.Get<Facade>();
        {
            facade.SetupDefaultSetting();
        }
    }

    #endregion Methods
}