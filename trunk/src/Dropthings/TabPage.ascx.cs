using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;

using Dropthings.Business;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.TabWorkflows;
using Dropthings.DataAccess;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;

public partial class TabPage : System.Web.UI.UserControl
{
    #region Fields

    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";

    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    #endregion Fields

    #region Properties

    public Dropthings.DataAccess.Page CurrentPage
    {
        get; set;
    }

    public List<Dropthings.DataAccess.Page> Pages
    {
        get; set;
    }

    public List<Dropthings.DataAccess.Page> LockedPages
    {
        get;
        set;
    }

    public Guid CurrentUserId
    {
        get;
        set;
    }

    #endregion Properties

    #region Methods

    public void LoadTabs(Guid currentUserId, List<Dropthings.DataAccess.Page> pages, List<Dropthings.DataAccess.Page> sharedPages, Dropthings.DataAccess.Page page)
    {
        this.CurrentPage = page;
        this.Pages = pages;
        this.LockedPages = sharedPages;
        this.CurrentUserId = currentUserId;
        this.SetupTabs();
    }

    public void RedirectToTab(Dropthings.DataAccess.Page page)
    {

        if (!page.IsLocked || CurrentUserId == page.UserId)
        {
            Response.Redirect('?' + page.UserTabName);
        }
        else
        {
            Response.Redirect('?' + page.LockedTabName);
        }
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void addNewTabLinkButton_Click(object sender, EventArgs e)
    {
        // -- Workflow way. Obselete.
        //var response = WorkflowHelper.Run<AddNewTabWorkflow, AddNewTabWorkflowRequest, AddNewTabWorkflowResponse>(
        //            new AddNewTabWorkflowRequest { LayoutType = "1", UserName = Profile.UserName }
        //        );

        //RedirectToTab(response.NewPage);

        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            var page = facade.CreatePage(string.Empty, null);
            RedirectToTab(page);
        }
    }

    private void SetupTabs()
    {
        tabList.Controls.Clear();

        var viewablePages = this.Pages;

        if (this.LockedPages != null)
        {
            viewablePages.AddRange(this.LockedPages);
        }
        
        foreach (var page in viewablePages)
        {
            var li = new HtmlGenericControl("li");
            li.ID = "Tab" + page.ID.ToString();
            li.Attributes["class"] = "tab " + (page.ID == CurrentPage.ID ? "activetab" : "inactivetab");

            var liWrapper = new HtmlGenericControl("div");
            li.Controls.Add(liWrapper);
            liWrapper.Attributes["class"] = "tab_wrapper";

            if (page.ID == CurrentPage.ID)
            {
                var tabTextDiv = new HtmlGenericControl("span");
                tabTextDiv.InnerText = page.Title;
                liWrapper.Controls.Add(tabTextDiv);
            }
            else
            {
                var url = "/?";

                if (!page.IsLocked || CurrentUserId == page.UserId)
                {
                    url += page.UserTabName;
                }
                else
                {
                    url += page.LockedTabName;
                }

                var tabLink = new HyperLink { Text = page.Title, NavigateUrl = url };
                liWrapper.Controls.Add(tabLink);
            }
            tabList.Controls.Add(li);
        }

        var addNewTabLinkButton = new LinkButton();
        addNewTabLinkButton.ID = "AddNewPage";
        addNewTabLinkButton.CssClass = "newtab_add newtab_add_block";
        addNewTabLinkButton.Click += new EventHandler(addNewTabLinkButton_Click);
        var li2 = new HtmlGenericControl("li");
        li2.Attributes["class"] = "newtab";
        li2.Controls.Add(addNewTabLinkButton);
        tabList.Controls.Add(li2);

        addNewTabLinkButton.Click += new EventHandler(addNewTabLinkButton_Click);

        // ILIAS 1/13/2009: Turning it off because new users are not getting the "Add New Page" link
        //if (Roles.Enabled)
        //{
        //    if (!Roles.IsUserInRole("ChangePageSettings"))
        //        addNewTabLinkButton.Visible = false;
        //}
    }

    #endregion Methods
}