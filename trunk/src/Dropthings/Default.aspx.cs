#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Workflow.Runtime;

using Dropthings.Business;
using Dropthings.Data;
using Dropthings.Web.Framework;
using Dropthings.Web.UI;
using Dropthings.Web.Util;
using Dropthings.Widget.Framework;
using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;
using Dropthings.Util;
using OmarALZabir.AspectF;

public partial class _Default : BasePage
{
    #region Fields

    private const string WIDGET_CONTAINER_CONTROL = "WidgetContainer.ascx";

    private UserSetup _Setup
    {
        get { return Context.Items[typeof(UserSetup)] as UserSetup; }
        set { Context.Items[typeof(UserSetup)] = value; }
    }

    #endregion Fields

    #region Methods

    /// <summary>
    /// Override the OnInit behavior to load the current user's pages and widgets. This
    /// needs to be done in OnInit, not on Page_Load because the ASP.NET control tree 
    /// is prepared after this and we need to create the widgets as dynamic controls. 
    /// So, we need to load the widgets and pages before the control tree creation starts.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        AspectF.Define
            .Retry(errors => errors.ToArray().Each(x => Response.Write(x.ToString())), Services.Get<ILogger>())
            .Log(Services.Get<ILogger>(), "OnInit: " + Profile.UserName)
            .Do(() =>
            {

                // Check if revisit is valid or not
                if (!base.IsPostBack)
                {
                    // Block cookie less visit attempts
                    if (Profile.IsFirstVisit)
                    {
                        if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.FirstVisit)) Response.End();
                    }
                    else
                    {
                        if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Revisit)) Response.End();
                    }
                }
                else
                {
                    // Limit number of postbacks
                    if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback)) Response.End();
                }
            });
    }


    /// <summary>
    /// Each widget generates its own unique ClientID bypassing default ASP.NET ID generation scheme.
    /// So, instead of ctl00_ctlMasterPage_defaultaspx_widgetcontainer001_Widget123 it generates clean
    /// and simple Widget123 as the control ID. However, in order to make postback and viewstate stuff
    /// work, the FindControl of the .aspx page needs to be overriden to let the WidgetPage handle the
    /// ID resolution first and if it can't then use the default ASP.NET FindControl.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override Control FindControl(string id)
    {        
        return this.WidgetTabHost.FindControl(id) ?? base.FindControl(id);
    }

    /// <summary>
    /// Override this to ensure the control tree is created including the tabs and the widgets
    /// before the LoadViewState is fired. Unless we do this at this time, the ViewState will fail
    /// to load for all dynamically created controls, which includes the Widgets.
    /// </summary>
    protected override void CreateChildControls()
    {
        var me = this;
        
        AspectF.Define
            .Retry(errors => errors.Each(x => Response.Write(x.ToString())), Services.Get<ILogger>())
            .Log(Services.Get<ILogger>(), "User visit: " + Profile.UserName)
            .Do(() => 
            {
                me.CallBaseCreateChildControl();
                me.LoadUserPageSetup(false);
                me.UserTabBar.LoadTabs(_Setup.CurrentUserId, _Setup.UserTabs, _Setup.UserSharedTabs, _Setup.CurrentTab, 
                    _Setup.IsTemplateUser);
                me.WidgetTabHost.LoadWidgets(_Setup.CurrentTab, WIDGET_CONTAINER_CONTROL);
                me.SetupTabControlPanel();
            });
    }

    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? Resources.SharedResources.NewTabTitle).TrimStart('?');

        AspectF.Define
            .Retry(Services.Get<ILogger>())
            .Do(() =>
        {
            var facade = Services.Get<Facade>();
            if (Profile.IsAnonymous)
            {
                if (Profile.IsFirstVisit)
                {
                    // First visit
                    Profile.IsFirstVisit = false;
                    Profile.Save();
                    _Setup = facade.FirstVisitHomeTab(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
                else
                {
                    _Setup = facade.RepeatVisitHomeTab(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
            }
            else
            {
                _Setup = facade.RepeatVisitHomeTab(Profile.UserName, pageTitle, false, Profile.IsFirstVisitAfterLogin);

                // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
                // to recrate the pages
                if (_Setup == null || _Setup.UserTabs == null || _Setup.UserTabs.Count() == 0)
                {
                    _Setup = facade.FirstVisitHomeTab(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
            }

            //save the profile to keep LastActivityAt updated
            Profile.LastActivityAt = DateTime.Now;
            Profile.IsFirstVisitAfterLogin = false;
            Profile.Save();
            
        });
    }

    private void SetupTabControlPanel()
    {
        this.TabControlPanel.InitTabs(_Setup.CurrentTab,
            _Setup.IsTemplateUser,
            _Setup.UserTabs.Count == 1,
            _Setup.CurrentUserId == _Setup.CurrentTab.AspNetUser.UserId,
            this.NewWidgetAdded);
    }

    private void CallBaseCreateChildControl()
    {
        base.CreateChildControls();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void OnReloadPage(object sender, EventArgs e)
    {
        this.ReloadCurrentPage();
    }

    private void ReloadCurrentPage()
    {
        this.LoadUserPageSetup(false);
        //this.SetupTabs();
        this.WidgetTabHost.LoadWidgets(_Setup.CurrentTab, WIDGET_CONTAINER_CONTROL);
    }

    public void NewWidgetAdded(WidgetInstance newWidget)
    {
        //this.ReloadCurrentPage();
        //this.WidgetTabHost.RefreshZone(newWidget.WidgetZone.ID);
        this.WidgetTabHost.AddNewWidget(newWidget);
    }
    
    #endregion Methods
}