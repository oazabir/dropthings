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
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.EntryPointWorkflows;
using Dropthings.Business.Workflows.TabWorkflows;
using Dropthings.Business.Workflows.WidgetWorkflows;
using Dropthings.DataAccess;
using Page = Dropthings.DataAccess.Page;
using Dropthings.Web.Framework;
using Dropthings.Web.UI;
using Dropthings.Web.Util;
using Dropthings.Widget.Framework;
using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;

public partial class _Default : BasePage
{
    #region Fields

    private const string WIDGET_CONTAINER_CONTROL = "WidgetContainer.ascx";

    #endregion Fields

    #region Properties

    protected string GetScriptVersion
    {
        get { return ConfigurationManager.AppSettings["ScriptVersionNo"]; }
    }

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if( val == null ) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
    }

    private bool ShouldOverrideCurrentPage
    {
        get { object val = ViewState["ShouldOverrideCurrentPage"]; if (val == null) return true; else return (bool)val; }
        set { ViewState["ShouldOverrideCurrentPage"] = value; }
    }

    //private UserVisitWorkflowResponse _Setup
    //{
    //    get { return Context.Items[typeof(UserVisitWorkflowResponse)] as UserVisitWorkflowResponse; }
    //    set { Context.Items[typeof(UserVisitWorkflowResponse)] = value; }
    //}

    private UserSetup _Setup
    {
        get { return Context.Items[typeof(UserSetup)] as UserSetup; }
        set { Context.Items[typeof(UserSetup)] = value; }
    }

    #endregion Properties

    #region Methods

    public override Control FindControl(string id)
    {
        var control = base.FindControl(id);
        if (control == null)
        {
            return this.WidgetPage.FindControl(id);
        }
        else
        {
            return control;
        }
    }

    protected void ChangeTabSettingsLinkButton_Clicked(object sender, EventArgs e)
    {
        if (this.ChangePageSettingsPanel.Visible)
            this.HideChangeSettingsPanel();
        else
            this.ShowChangeSettingsPanel();
    }

    protected override void CreateChildControls()
    {
        TrapDatabaseException(() =>
            {
                base.CreateChildControls();
                this.LoadUserPageSetup(false);
                this.LoadAddStuff();
                this.UserTabPage.LoadTabs(_Setup.CurrentUserId, _Setup.UserPages, _Setup.UserSharedPages, _Setup.CurrentPage);
                this.WidgetPage.LoadWidgets(_Setup.CurrentPage, WIDGET_CONTAINER_CONTROL);
                this.LockPageContent(_Setup.CurrentPage);
                this.LoadOptionsForTemplateUser();
            });
    }

    private void LoadOptionsForTemplateUser()
    {
        if(_Setup.RoleTemplate.TemplateUserId == _Setup.CurrentUserId)
        {
            pnlTemplateUserSettings.Visible = true;
        }
    }

    protected void DeleteTabLinkButton_Clicked(object sender, EventArgs e)
    {
        TrapDatabaseException(() =>
            {
                using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                {
                    var newCurrentPage = facade.DeletePage(_Setup.CurrentPage.ID);
                    Context.Cache.Remove(Profile.UserName);
                    RedirectToTab(newCurrentPage);
                }
            });
    }

    public void RedirectToTab(Dropthings.DataAccess.Page page)
    {
        if (!page.IsLocked)
        {
            Response.Redirect('?' + page.UserTabName);
        }
        else
        {
            Response.Redirect('?' + page.LockedTabName);
        }
    }

    protected void HideAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = false;
        this.HideAddContentPanel.Visible = false;
        this.ShowAddContentPanel.Visible = true;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        TrapDatabaseException(() =>
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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (this.AddContentPanel.Visible)
            ScriptManager.RegisterStartupScript(this.AddContentPanel, typeof(Panel), "ShowAddContentPanel" + DateTime.Now.Ticks.ToString(),
                "DropthingsUI.showWidgetGallery();", true);

//        if (!Page.IsPostBack)
//            ScriptManager.RegisterStartupScript(this, typeof(Page), "OverrideIsScriptLoaded", @"
//                if(typeof(Sys)!=='undefined')
//                {
//                    if(typeof(Sys._ScriptLoader) !== 'undefined')
//                    {
//                        Sys._ScriptLoader.isScriptLoaded = function Sys$_ScriptLoader$isScriptLoaded(scriptSrc)
//                        {
//                            var dummyScript = document.createElement('script');
//                            dummyScript.src = scriptSrc;
//                            var fullUrl = dummyScript.src;
//                            var result = Array.contains(Sys._ScriptLoader._getLoadedScripts(), fullUrl);
//                            if (result === true) return true;
//                            result = Array.contains(window._combinedScripts, fullUrl);
//                            if (result === true) return true;
//                            var scriptTags = document.getElementsByTagName('script');
//                            for(var i = 0; i < scriptTags.length; i ++ ) if (scriptTags[i].src == fullUrl) return true;
//                            return false;
//                        }
//                    }
//                }", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveNewTitleButton_Clicked(object sender, EventArgs e)
    {
        var newTitle = this.NewTitleTextBox.Text.Trim();

        if (newTitle != _Setup.CurrentPage.Title)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ChangePageName(newTitle);
            }

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
    }

    protected void SaveTabLockSettingButton_Clicked(object sender, EventArgs e)
    {
        var isLocked = this.TabLocked.Checked;

        if (isLocked != _Setup.CurrentPage.IsLocked)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                if(isLocked)
                {
                    facade.LockPage();
                }
                else
                {
                    facade.UnLockPage();
                }
            }

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
    }

    protected void SaveTabMaintenenceSettingButton_Clicked(object sender, EventArgs e)
    {
        var isInMaintenenceModeLocked = this.TabMaintanance.Checked;

        if (isInMaintenenceModeLocked != _Setup.CurrentPage.IsDownForMaintenance)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ChangePageMaintenenceStatus(isInMaintenenceModeLocked);
            }

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
    }

    protected void SaveTabServeAsStartPageSettingButton_Clicked(object sender, EventArgs e)
    {
        var shouldServeAsStartPage = this.TabServeAsStartPage.Checked;

        if (shouldServeAsStartPage != _Setup.CurrentPage.ServeAsStartPageAfterLogin.GetValueOrDefault())
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ChangeServeAsStartPageAfterLoginStatus(shouldServeAsStartPage);
            }

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
    }

    protected void ShowAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = true;
        this.HideAddContentPanel.Visible = true;
        this.ShowAddContentPanel.Visible = false;

        this.LoadAddStuff();
    }

    private void HideChangeSettingsPanel()
    {
        this.ChangePageSettingsPanel.Visible = false;
        this.ChangePageTitleLinkButton.Text = "Change Settings";
    }

    private void LoadAddStuff()
    {
        this.WidgetListControlAdd.LoadWidgetList(newWidget =>
        {
            this.ReloadCurrentPage();
            this.WidgetPage.RefreshZone(newWidget.WidgetZoneId);
        });
    }

    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? string.Empty).TrimStart('?');

        TrapDatabaseException(() =>
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                if (Profile.IsAnonymous)
                {
                    if (Profile.IsFirstVisit)
                    {
                        // First visit
                        Profile.IsFirstVisit = false;
                        Profile.Save();
                        _Setup = facade.FirstVisitHomePage(Profile.UserName, pageTitle, true);
                    }
                    else
                    {
                        _Setup = facade.RepeatVisitHomePage(Profile.UserName, pageTitle, true, Profile.LastActivityAt);
                    }
                }
                else
                {
                    _Setup = facade.RepeatVisitHomePage(Profile.UserName, pageTitle, false, Profile.LastActivityAt);

                    // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
                    // to recrate the pages
                    if (_Setup == null || _Setup.UserPages == null || _Setup.UserPages.Count == 0)
                    {
                        _Setup = facade.FirstVisitHomePage(Profile.UserName, pageTitle, true);
                    }
                }

                //save the profile to keep LastActivityAt updated
                Profile.LastActivityAt = DateTime.Now;
                Profile.Save();
            }
        });


        // Workflow way -- obselete

        //TrapDatabaseException(() =>
        //{
        //    if (Profile.IsAnonymous)
        //    {
        //        if (Profile.IsFirstVisit)
        //        {
        //            // First visit
        //            Profile.IsFirstVisit = false;
        //            Profile.Save();

        //            //_Setup = WorkflowHelper.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
        //            //    new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName });
        //            _Setup = new DashboardFacade(Profile.UserName).SetupNewUser(Profile.UserName);

        //        }
        //        else
        //        {
        //            _Setup = WorkflowHelper.Run<UserVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
        //                new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = true });
        //        }
        //    }
        //    else
        //    {
        //        _Setup = WorkflowHelper.Run<UserVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
        //            new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = false });

        //        // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
        //        // to recrate the pages
        //        if (_Setup == null || _Setup.UserPages == null || _Setup.UserPages.Count == 0)
        //        {
        //            _Setup = WorkflowHelper.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
        //                new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName, IsAnonymous = false });
        //        }
        //    }
        //});
    }

    private void OnReloadPage(object sender, EventArgs e)
    {
        this.ReloadCurrentPage();
    }

    private void ReloadCurrentPage()
    {
        this.LoadUserPageSetup(false);
        //this.SetupTabs();
        this.WidgetPage.LoadWidgets(_Setup.CurrentPage, WIDGET_CONTAINER_CONTROL);
    }

    private void ShowChangeSettingsPanel()
    {
        //if tab counts 1 or less deleting disabled
        if (_Setup.UserPages.Count <= 1)
            this.DeleteTabLinkButton.Enabled = false;

        this.ChangePageSettingsPanel.Visible = true;
        this.ChangePageTitleLinkButton.Text = "Hide Settings";

        this.NewTitleTextBox.Text = _Setup.CurrentPage.Title;
        this.TabLocked.Checked = _Setup.CurrentPage.IsLocked;

        //show options for the maintenence mode
        maintenenceOption.Visible = _Setup.CurrentPage.IsLocked;
        this.TabMaintanance.Checked = _Setup.CurrentPage.IsDownForMaintenance;
        this.TabServeAsStartPage.Checked = _Setup.CurrentPage.ServeAsStartPageAfterLogin.GetValueOrDefault();
    }

    private void TrapDatabaseException(Action work)
    {
        try
        {
            work();
        }
        catch (Exception x)
        {
            Response.Write("<html>");
            Response.Write("<h1>Error occured loading the page. Please ensure you have run the <a href='Setup.aspx'>Setup page</a>.</h1>");
            Response.Write("<pre>");
            Response.Write(x.ToString());
            Response.Write("</pre>");
            Response.Write("</html>");
        }
    }

    private void LockPageContent(Page page)
    {
        if (page.UserId != _Setup.CurrentUserId)
        {
            ShowAddContentPanel.Enabled = HideAddContentPanel.Enabled = ChangePageTitleLinkButton.Enabled = !page.IsLocked;
        }
    }

    #endregion Methods
}