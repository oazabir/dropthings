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

using Dropthings.Util;

public partial class TabBar : System.Web.UI.UserControl
{
    #region Fields

    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";

    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    #endregion Fields

    #region Properties

    public Tab CurrentTab
    {
        get; set;
    }

    public IEnumerable<Tab> Pages
    {
        get; set;
    }

    public List<Tab> LockedTabs
    {
        get;
        set;
    }

    public Guid CurrentUserId
    {
        get;
        set;
    }

    public bool IsTemplateUser { get; set; }

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


    #endregion Properties

    #region Methods

    public void LoadTabs(Guid currentUserId, IEnumerable<Tab> tabs, List<Tab> sharedTabs, Tab tab, bool isTemplateUser)
    {
        this.IsTemplateUser = isTemplateUser;
        this.CurrentTab = tab;
        this.Pages = tabs;
        this.LockedTabs = sharedTabs;
        this.CurrentUserId = currentUserId;
        this.SetupTabs();
    }

    public void RedirectToTab(Tab page)
    {
        if (!page.IsLocked || CurrentUserId == page.AspNetUser.UserId)
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

        var facade = Services.Get<Facade>();
        {
            var page = facade.CreateTab(Resources.SharedResources.NewTabTitle, 0);
            RedirectToTab(page);
        }
    }

    private void SetupTabs()
    {
        tabList.Controls.Clear();
        var facade = Services.Get<Facade>();

        // True if the currently logged in user is the user defined in web.config
        // as the template user for the registered user template or anonymous user template
        var templateSetting = facade.GetUserSettingTemplate();
        var isTemplateUser = !Profile.IsAnonymous
            && (Profile.UserName.IsSameAs(templateSetting.RegisteredUserSettingTemplate.UserName)
            || Profile.UserName.IsSameAs(templateSetting.AnonUserSettingTemplate.UserName));

        var viewablePages = this.Pages.ToList();

        if (this.LockedTabs != null)
        {
            viewablePages.AddRange(this.LockedTabs);
            viewablePages = viewablePages.OrderBy(o => o.OrderNo.GetValueOrDefault()).ToList();
        }
        
        foreach (var page in viewablePages)
        {
            var li = new HtmlGenericControl("li");
            li.ID = "Tab" + page.ID.ToString();
            li.Attributes["class"] = "tab " + (page.ID == CurrentTab.ID ? "activetab" : "inactivetab") +
                                     (page.IsLocked && !isTemplateUser ? " nodrag" : string.Empty);

            var liWrapper = new HtmlGenericControl("div");
            li.Controls.Add(liWrapper);
            liWrapper.Attributes["class"] = "tab_wrapper";

            if (page.ID == CurrentTab.ID)
            {
                var tabTextDiv = new HtmlGenericControl("span");
                tabTextDiv.Attributes["class"] = "current_tab";
                tabTextDiv.InnerText = page.Title;
                liWrapper.Controls.Add(tabTextDiv);
            }
            else
            {
                var url = "?";

                if (!page.IsLocked || CurrentUserId == page.AspNetUser.UserId)
                {
                    url += page.UserTabName;
                }
                else
                {
                    url += page.LockedTabName;
                }

                var tabLink = new HyperLink { Text = page.Title, NavigateUrl = url, CssClass = "tab_link" };
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