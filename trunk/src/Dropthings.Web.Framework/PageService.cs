#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for WidgetService
/// </summary>
namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Workflow.Runtime;

    using Dropthings.Business;
    using Dropthings.DataAccess;
    using Dropthings.Web.Framework;

    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;
    using Dropthings.Util;

    public class PageService : WebServiceBase
    {
        #region Constructors

        public PageService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            AspectF.Define.HowLong(Services.Get<ILogger>(),
                "Begin: ChangePageLayout {0}".FormatWith(newLayout), "End: ChangePageLayout {0}")
                .Do(() =>
                    {
                        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                        {
                            facade.ModifyPageLayout(newLayout);
                        }
                    });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveTab(int pageId, int orderNo)
        {
            AspectF.Define.HowLong(Services.Get<ILogger>(),
                "Begin: MoveTab {0} {1}".FormatWith(pageId, orderNo), "End: MoveTab {0}")
                .Do(() =>
                    {
                        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                        {
                            facade.MovePage(pageId, orderNo);
                        }
                    });
        }

        #endregion Methods
    }
}