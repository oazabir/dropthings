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
using Page = Dropthings.Data.Page;
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

    #endregion Fields

    #region Properties

    public bool IsTemplateUser
    {
        get { return Convert.ToBoolean(ViewState[this.ClientID + "_IsTemplateUser"]); }
        set { ViewState[this.ClientID + "_IsTemplateUser"] = value; }
    }

    public bool EnableTabSorting
    {
        get 
        {
            if (!ConstantHelper.EnableTabSorting && IsTemplateUser)
            {
                return ConstantHelper.EnableTabSorting;
            }
            
            return ConstantHelper.EnableTabSorting;
        }
    }

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if( val == null ) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
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
        return this.WidgetPage.FindControl(id) ?? base.FindControl(id);
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
            .Retry(errors => errors.ToArray().Each(x => Response.Write(x.ToString())), Services.Get<ILogger>())
            .Log(Services.Get<ILogger>(), "User visit: " + Profile.UserName)
            .Do(() => {
                me.CallBaseCreateChildControl();
                me.LoadUserPageSetup(false);
                me.LoadAddStuff();
                me.UserTabPage.LoadTabs(_Setup.CurrentUserId, _Setup.UserPages, _Setup.UserSharedPages, _Setup.CurrentPage);
                me.WidgetPage.LoadWidgets(_Setup.CurrentPage, WIDGET_CONTAINER_CONTROL);
                me.LockPageContent(_Setup.CurrentPage);
                me.LoadOptionsForTemplateUser();
            });
    }

    private void CallBaseCreateChildControl()
    {
        base.CreateChildControls();
    }

    
    private void LoadOptionsForTemplateUser()
    {
        if(_Setup.RoleTemplate.aspnet_Users.UserId == _Setup.CurrentUserId)
        {
            pnlTemplateUserSettings.Visible = true;
        }
    }

    protected void ChangeTabSettingsLinkButton_Clicked(object sender, EventArgs e)
    {
        if (this.ChangePageSettingsPanel.Visible)
            this.HideChangeSettingsPanel();
        else
            this.ShowChangeSettingsPanel();
    }

    
    protected void DeleteTabLinkButton_Clicked(object sender, EventArgs e)
    {
        AspectF.Define.TrapLogThrow(Services.Get<ILogger>()).Do(() =>
            {
                var facade = Services.Get<Facade>();
                {
                    var newCurrentPage = facade.DeletePage(_Setup.CurrentPage.ID);
                    RedirectToTab(newCurrentPage);
                }
            });
    }

    public void RedirectToTab(Page page)
    {
        if (!page.IsLocked)
        {
            Response.Redirect("Default.aspx?" + page.UserTabName);
        }
        else
        {
            Response.Redirect("Default.aspx?" + page.LockedTabName);
        }
    }

    protected void HideAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = false;
        this.HideAddContentPanel.Visible = false;
        this.ShowAddContentPanel.Visible = true;
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (_Setup != null)
        {
            IsTemplateUser = _Setup.RoleTemplate.aspnet_Users.UserId.Equals(_Setup.CurrentUserId);  
        }

        if (this.AddContentPanel.Visible)
            ScriptManager.RegisterStartupScript(this.AddContentPanel, typeof(Panel), "ShowAddContentPanel" + DateTime.Now.Ticks.ToString(),
                "DropthingsUI.showWidgetGallery();", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SaveNewTitleButton_Clicked(object sender, EventArgs e)
    {
        var newTitle = this.NewTitleTextBox.Text.Trim();

        if (newTitle != _Setup.CurrentPage.Title)
        {
            var facade = Services.Get<Facade>();
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
            var facade = Services.Get<Facade>();
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
            var facade = Services.Get<Facade>();
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
            var facade = Services.Get<Facade>();
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
        this.ChangePageTitleLinkButton.Text = (String)GetGlobalResourceObject("SharedResources", "ChangeSettings");
    }

    private void LoadAddStuff()
    {
        this.WidgetListControlAdd.LoadWidgetList(newWidget =>
        {
            this.ReloadCurrentPage();
            this.WidgetPage.RefreshZone(newWidget.WidgetZone.ID);
        });
    }

    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? string.Empty).TrimStart('?');

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
                    _Setup = facade.FirstVisitHomePage(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
                else
                {
                    _Setup = facade.RepeatVisitHomePage(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
            }
            else
            {
                _Setup = facade.RepeatVisitHomePage(Profile.UserName, pageTitle, false, Profile.IsFirstVisitAfterLogin);

                // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
                // to recrate the pages
                if (_Setup == null || _Setup.UserPages == null || _Setup.UserPages.Count() == 0)
                {
                    _Setup = facade.FirstVisitHomePage(Profile.UserName, pageTitle, true, Profile.IsFirstVisitAfterLogin);
                }
            }

            //save the profile to keep LastActivityAt updated
            Profile.LastActivityAt = DateTime.Now;
            Profile.IsFirstVisitAfterLogin = false;
            Profile.Save();
            
        });
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
        if (_Setup.UserPages.Count() <= 1)
            this.DeleteTabLinkButton.Enabled = false;

        this.ChangePageSettingsPanel.Visible = true;
        this.ChangePageTitleLinkButton.Text = (String)GetGlobalResourceObject("SharedResources", "HideSettings");

        this.NewTitleTextBox.Text = _Setup.CurrentPage.Title;
        this.TabLocked.Checked = _Setup.CurrentPage.IsLocked;

        //show options for the maintenence mode
        this.maintenenceOption.Visible = _Setup.CurrentPage.IsLocked;
        this.TabMaintanance.Checked = _Setup.CurrentPage.IsDownForMaintenance;
        this.serveAsStartPageOption.Visible = _Setup.CurrentPage.IsLocked && _Setup.IsRoleTemplateForRegisterUser;
        this.TabServeAsStartPage.Checked = _Setup.CurrentPage.ServeAsStartPageAfterLogin.GetValueOrDefault();
    }

    private void LockPageContent(Page page)
    {
        if (page.aspnet_Users.UserId != _Setup.CurrentUserId)
        {
            ShowAddContentPanel.Enabled = HideAddContentPanel.Enabled = ChangePageTitleLinkButton.Enabled = !page.IsLocked;
        }
    }


    protected void Layout1_Clicked(object sender, EventArgs e)
    {
        Services.Get<Facade>().ModifyPageLayout(1);
        ReloadPage();
    }
    protected void Layout2_Clicked(object sender, EventArgs e)
    {
        Services.Get<Facade>().ModifyPageLayout(2);
        ReloadPage();
    }
    protected void Layout3_Clicked(object sender, EventArgs e)
    {
        Services.Get<Facade>().ModifyPageLayout(3);
        ReloadPage();
    }
    protected void Layout4_Clicked(object sender, EventArgs e)
    {
        Services.Get<Facade>().ModifyPageLayout(4);
        ReloadPage();
    }

    private void ReloadPage()
    {
        Response.Redirect(Request.Url.ToString());
    }
    #endregion Methods
}