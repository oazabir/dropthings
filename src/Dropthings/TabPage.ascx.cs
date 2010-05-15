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
using Dropthings.Data;
using Dropthings.Web.Framework;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;

using Page = Dropthings.Data.Page;

public partial class TabPage : System.Web.UI.UserControl
{
    #region Fields

    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";

    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    #endregion Fields

    #region Properties

    public Page CurrentPage
    {
        get; set;
    }

    public IEnumerable<Page> Pages
    {
        get; set;
    }

    public List<Page> LockedPages
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

    public void LoadTabs(Guid currentUserId, IEnumerable<Page> pages, List<Page> sharedPages, Page page)
    {
        this.CurrentPage = page;
        this.Pages = pages;
        this.LockedPages = sharedPages;
        this.CurrentUserId = currentUserId;
        this.SetupTabs();
    }

    public void RedirectToTab(Page page)
    {
        if (!page.IsLocked || CurrentUserId == page.aspnet_Users.UserId)
        {
            Response.Redirect("Default.aspx?" + page.UserTabName);
        }
        else
        {
            Response.Redirect("Default.aspx?" + page.LockedTabName);
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

        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            var page = facade.CreatePage(string.Empty, 0);
            RedirectToTab(page);
        }
    }

    private void SetupTabs()
    {
        tabList.Controls.Clear();
        bool isTemplateUser = false;
        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            var roleTemplate = facade.GetRoleTemplate(CurrentUserId);
            isTemplateUser = roleTemplate.aspnet_Users.UserId.Equals(CurrentUserId);
        }

        var viewablePages = this.Pages.ToList();

        if (this.LockedPages != null)
        {
            viewablePages.AddRange(this.LockedPages);
            viewablePages = viewablePages.OrderBy(o => o.OrderNo.GetValueOrDefault()).ToList();
        }
        
        foreach (var page in viewablePages)
        {
            var li = new HtmlGenericControl("li");
            li.ID = "Tab" + page.ID.ToString();
            li.Attributes["class"] = "tab " + (page.ID == CurrentPage.ID ? "activetab" : "inactivetab") +
                                     (page.IsLocked && !isTemplateUser ? " nodrag" : string.Empty);

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
                var url = "?";

                if (!page.IsLocked || CurrentUserId == page.aspnet_Users.UserId)
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