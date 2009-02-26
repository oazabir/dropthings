// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;

using System.Linq;
using System.Data.Linq;

using Dropthings.Business;
using Dropthings.DataAccess;

using Page = Dropthings.DataAccess.Page;
using Dropthings.Web.Util;
using Dropthings.Web.UI;
using Dropthings.Business.Workflows.EntryPointWorkflows;
using Dropthings.Business.Workflows.TabWorkflows;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows.WidgetWorkflows;

public partial class _Default : BasePage
{
    private const string WIDGET_CONTAINER_CONTROL = "WidgetContainer.ascx";

    protected string GetScriptVersion
    {
        get { return ConfigurationManager.AppSettings["ScriptVersionNo"]; }
    }
    private IUserVisitWorkflowResponse _Setup
    {
        get { return Context.Items[typeof(IUserVisitWorkflowResponse)] as IUserVisitWorkflowResponse; }
        set { Context.Items[typeof(IUserVisitWorkflowResponse)] = value; }
    }    
    
    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if( val == null ) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
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

    protected override void CreateChildControls()
    {
        TrapDatabaseException(() =>
            {
                base.CreateChildControls();
                this.LoadUserPageSetup(false);
                this.LoadAddStuff();
                this.UserTabPage.LoadTabs(_Setup.UserPages, _Setup.CurrentPage);

                this.WidgetPage.LoadWidgets(_Setup.CurrentPage, WIDGET_CONTAINER_CONTROL);
            });
    }


    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? string.Empty).TrimStart('?');

        TrapDatabaseException(() =>
        {
            if (Profile.IsAnonymous)
            {
                if (Profile.IsFirstVisit)
                {
                    // First visit
                    Profile.IsFirstVisit = false;
                    Profile.Save();

                    _Setup = RunWorkflow.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                        new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName });

                }
                else
                {
                    _Setup = RunWorkflow.Run<UserVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                        new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = true });
                }
            }
            else
            {
                _Setup = RunWorkflow.Run<UserVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                    new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = false });

                // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
                // to recrate the pages
                if (_Setup == null || _Setup.UserPages == null || _Setup.UserPages.Count == 0)
                {
                    _Setup = RunWorkflow.Run<FirstVisitWorkflow, UserVisitWorkflowRequest, UserVisitWorkflowResponse>(
                        new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName, IsAnonymous = false });
                }
            }
        });
    }

    
    
    private void RedirectToTab(Page page)
    {
        Response.Redirect('?' + page.TabName());
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

    protected void ShowAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = true;
        this.HideAddContentPanel.Visible = true;
        this.ShowAddContentPanel.Visible = false;

        this.LoadAddStuff();        
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (this.AddContentPanel.Visible)
            ScriptManager.RegisterStartupScript(this.AddContentPanel, typeof(Panel), "ShowAddContentPanel" + DateTime.Now.Ticks.ToString(),
                "DropthingsUI.showWidgetGallery();", true);
    }
    
    protected void HideAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = false;
        this.HideAddContentPanel.Visible = false;
        this.ShowAddContentPanel.Visible = true;
    }

    private void LoadAddStuff()
    {
        this.WidgetListControlAdd.LoadWidgetList(newWidget =>
        {
            this.ReloadCurrentPage();
            this.WidgetPage.RefreshZone(newWidget.WidgetZoneId);
        });
    }   

    protected void ChangeTabSettingsLinkButton_Clicked(object sender, EventArgs e)
    {
        if (this.ChangePageSettingsPanel.Visible)
            this.HideChangeSettingsPanel();
        else
            this.ShowChangeSettingsPanel();
    }

    protected void SaveNewTitleButton_Clicked(object sender, EventArgs e)
    {
        var newTitle = this.NewTitleTextBox.Text.Trim();

        if (newTitle != _Setup.CurrentPage.Title)
        {
            var response = RunWorkflow.Run<ChangePageNameWorkflow, ChangeTabNameWorkflowRequest, ChangeTabNameWorkflowResponse>(
                            new ChangeTabNameWorkflowRequest { PageName = newTitle, UserName = Profile.UserName }
                        );
            
            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
        
    }

    protected void DeleteTabLinkButton_Clicked(object sender, EventArgs e)
    {
        var response = RunWorkflow.Run<DeletePageWorkflow, DeleteTabWorkflowRequest, DeleteTabWorkflowResponse>(
                            new DeleteTabWorkflowRequest { PageID = _Setup.CurrentPage.ID, UserName = Profile.UserName }
                        );

        Context.Cache.Remove(Profile.UserName);

        RedirectToTab(response.NewCurrentPage);
    }

    private void ShowChangeSettingsPanel()
    {
        //if tab counts 1 or less deleting disabled
        if (_Setup.UserPages.Count <= 1)
            this.DeleteTabLinkButton.Enabled = false;

        this.ChangePageSettingsPanel.Visible = true;
        this.ChangePageTitleLinkButton.Text = "Hide Settings";

        this.NewTitleTextBox.Text = _Setup.CurrentPage.Title;
    }

    private void HideChangeSettingsPanel()
    {
        this.ChangePageSettingsPanel.Visible = false;
        this.ChangePageTitleLinkButton.Text = "Change Settings";
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
}