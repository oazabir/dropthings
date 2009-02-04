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

        // Check if revisit is valid or not
        if( !base.IsPostBack ) 
        {
            // Block cookie less visit attempts
            if( Profile.IsFirstVisit )
            {
                if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.FirstVisit))  Response.End();
            }
            else
            {
                if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Revisit) )  Response.End();
            }
        }
        else
        {
            // Limit number of postbacks
            if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback) ) Response.End();
        }
        // OMAR 1/13/2009: Turning it off because new users are not getting the "Add Stuff" and "Change Settings" links
        //if (Roles.Enabled)
        //{
        //    if (!Roles.IsUserInRole("AddRemovePageWidgets"))
        //        this.ShowAddContentPanel.Visible = false;

        //    if (!Roles.IsUserInRole("ChangePageSettings"))
        //        this.ChangePageTitleLinkButton.Visible = false;
        //}
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();
        this.LoadUserPageSetup(false);
        this.LoadAddStuff();
        this.UserTabPage.LoadTabs(_Setup.UserPages, _Setup.CurrentPage);

        //this.WidgetPage.OnReloadPage += new EventHandler(this.OnReloadPage);
        this.WidgetPage.LoadWidgets(_Setup.CurrentPage, wi => !ScriptManager.GetCurrent(Page).IsInAsyncPostBack, WIDGET_CONTAINER_CONTROL);
        //this.WidgetPage.LoadWidgets(_Setup.CurrentPage, _Setup.WidgetInstances, wi => !ScriptManager.GetCurrent(Page).IsInAsyncPostBack, WIDGET_CONTAINER_CONTROL);
    }


    private void LoadUserPageSetup(bool noCache)
    {
        // If URL has the page title, load that page by default
        string pageTitle = (Request.Url.Query ?? string.Empty).TrimStart('?');

        if( Profile.IsAnonymous )
        {
            if( Profile.IsFirstVisit )
            {
                // First visit
                Profile.IsFirstVisit = false;
                Profile.Save();

                _Setup = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        FirstVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName }
                    );

                //_Setup = new DashboardFacade(Profile.UserName).NewUserVisit();
            }
            else
            {
                //_Setup = new DashboardFacade(Profile.UserName).LoadUserSetup(pageTitle);
                _Setup = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        UserVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = true }
                        );
            }
        }
        else
        {
            //_Setup = new DashboardFacade(Profile.UserName).LoadUserSetup(pageTitle);

            _Setup = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        UserVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new UserVisitWorkflowRequest { PageName = pageTitle, UserName = Profile.UserName, IsAnonymous = false }
                        );

            // OMAR: If user's cookie remained in browser but the database was changed, there will be no pages. So, we need
            // to recrate the pages
            if (_Setup == null || _Setup.UserPages == null || _Setup.UserPages.Count == 0)
            {
                _Setup = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        FirstVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new UserVisitWorkflowRequest { PageName = string.Empty, UserName = Profile.UserName, IsAnonymous = false }
                    );
            }
        }
    }

    
    
    private void RedirectToTab(Page page)
    {
        Response.Redirect('?' + page.TabName());
    }

    //void addNewTabLinkButton_Click(object sender, EventArgs e)
    //{
    //    var page = new DashboardFacade(Profile.UserName).AddNewPage("1");
    //    RedirectToTab(page);
    //}
    
    private void OnReloadPage(object sender, EventArgs e)
    {
        this.ReloadCurrentPage(wi => false);
    }
    private void ReloadCurrentPage(Func<WidgetInstance, bool> isWidgetFirstLoad)
    {
        this.LoadUserPageSetup(false);
        //this.SetupTabs();

        this.WidgetPage.LoadWidgets(_Setup.CurrentPage, isWidgetFirstLoad, WIDGET_CONTAINER_CONTROL);
    }

    protected void ShowAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = true;
        this.HideAddContentPanel.Visible = true;
        this.ShowAddContentPanel.Visible = false;

        this.LoadAddStuff();
    }
    
    protected void HideAddContentPanel_Click(object sender, EventArgs e)
    {
        this.AddContentPanel.Visible = false;
        this.HideAddContentPanel.Visible = false;
        this.ShowAddContentPanel.Visible = true;
    }

    //private List<Widget> WidgetList
    //{
    //    get
    //    {
    //        List<Widget> widgets = Cache["Widgets"] as List<Widget>;
    //        if( null == widgets )
    //        {
    //            widgets = new DashboardFacade(Profile.UserName).GetWidgetList();
    //            Cache["Widgets"] = widgets;
    //        }
        
    //        return widgets;
    //    }
    //}

    private void LoadAddStuff()
    {
        this.WidgetListControlAdd.LoadWidgetList(newWidget =>
        {
            this.ReloadCurrentPage(wi => wi.Id == newWidget.Id);
            this.WidgetPage.RefreshZone(newWidget.WidgetZoneId);
        });
    }

    //void WidgetDataList_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    if( !ActionValidator.IsValid(ActionValidator.ActionTypeEnum.AddNewWidget) ) return;

    //    int widgetId = int.Parse( e.CommandArgument.ToString() );

    //    DashboardFacade facade = new DashboardFacade(Profile.UserName);
    //    WidgetInstance newWidget = facade.AddWidget( widgetId, 0, 0, 0 );

    //    /// User added a new widget. The new widget is loaded for the first time. So, it's not 
    //    /// a postback experience for the widget. But for rest of the widgets, it is a postback experience.
    //    this.ReloadCurrentPage(wi => wi.Id == newWidget.Id);
    //}

    //void WidgetDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    if( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
    //    {
    //        Widget widget = e.Item.DataItem as Widget;

    //        LinkButton link = e.Item.Controls.OfType<LinkButton>().Single();

    //        link.Text = widget.Name;

    //        link.CommandName = "AddWidget";
    //        link.CommandArgument = widget.ID.ToString();
    //    }
    //}

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
            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ChangePageNameWorkflow,
                        ChangeTabNameWorkflowRequest,
                        ChangeTabNameWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ChangeTabNameWorkflowRequest { PageName = newTitle, UserName = Profile.UserName }
                        );
            //new DashboardFacade(Profile.UserName).ChangePageName(newTitle);

            this.LoadUserPageSetup(false);

            RedirectToTab(_Setup.CurrentPage);
        }
        
    }

    protected void DeleteTabLinkButton_Clicked(object sender, EventArgs e)
    {
        //var currentPage = new DashboardFacade(Profile.UserName).DeleteCurrentPage(_Setup.CurrentPage.ID);

        var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeletePageWorkflow,
                        DeleteTabWorkflowRequest,
                        DeleteTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
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
        this.ChangePageTitleLinkButton.Text = "Change Settings �";
    }
}