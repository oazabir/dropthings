using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Util;
using Dropthings.Business.Facade;
using Page = Dropthings.Data.Page;
using OmarALZabir.AspectF;
using Dropthings.Data;

public partial class ChangeSettingsControl : System.Web.UI.UserControl
{

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if (val == null) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
    }

    public Page CurrentPage { get; set; }
    public bool IsTemplateUser { get; set; }
    public bool IsOnlyPage { get; set; }
    public bool IsOwner { get; set; }
    
    public delegate void NewWidgetAddedDelegate(WidgetInstance wi);
    private NewWidgetAddedDelegate _OnNewWidgetCallback;
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Init(Page currentPage, bool isTemplateUser, bool isOnlyPage, bool isOwner, NewWidgetAddedDelegate onNewWidget)
    {
        this.IsTemplateUser = isTemplateUser;
        this.IsOnlyPage = isOnlyPage;
        this.IsOwner = isOwner;
        this.CurrentPage = currentPage;
        this._OnNewWidgetCallback = onNewWidget;

        this.LoadAddStuff();
        this.LockPageContent();
        this.LoadOptionsForTemplateUser();
    }

    protected void SaveNewTitleButton_Clicked(object sender, EventArgs e)
    {
        var newTitle = this.NewTitleTextBox.Text.Trim();

        if (newTitle != this.CurrentPage.Title)
        {
            var facade = Services.Get<Facade>();
            {
                facade.ChangePageName(newTitle);
            }

            ReloadPage();
        }
    }

    protected void SaveTabLockSettingButton_Clicked(object sender, EventArgs e)
    {
        var isLocked = this.TabLocked.Checked;

        if (isLocked != this.CurrentPage.IsLocked)
        {
            var facade = Services.Get<Facade>();
            {
                if (isLocked)
                {
                    facade.LockPage();
                }
                else
                {
                    facade.UnLockPage();
                }
            }

            ReloadPage();
        }
    }

    protected void SaveTabMaintenenceSettingButton_Clicked(object sender, EventArgs e)
    {
        var isInMaintenenceModeLocked = this.TabMaintanance.Checked;

        if (isInMaintenenceModeLocked != this.CurrentPage.IsDownForMaintenance)
        {
            var facade = Services.Get<Facade>();
            {
                facade.ChangePageMaintenenceStatus(isInMaintenenceModeLocked);
            }

            ReloadPage();
        }
    }

    protected void SaveTabServeAsStartPageSettingButton_Clicked(object sender, EventArgs e)
    {
        var shouldServeAsStartPage = this.TabServeAsStartPage.Checked;

        if (shouldServeAsStartPage != this.CurrentPage.ServeAsStartPageAfterLogin.GetValueOrDefault())
        {
            var facade = Services.Get<Facade>();
            {
                facade.ChangeServeAsStartPageAfterLoginStatus(shouldServeAsStartPage);
            }

            ReloadPage();
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

    
    private void ShowChangeSettingsPanel()
    {
        //if tab counts 1 or less deleting disabled
        if (this.IsOnlyPage)
            this.DeleteTabLinkButton.Enabled = false;

        this.ChangePageSettingsPanel.Visible = true;
        this.ChangePageTitleLinkButton.Text = (String)GetGlobalResourceObject("SharedResources", "HideSettings");

        this.NewTitleTextBox.Text = this.CurrentPage.Title;
        this.TabLocked.Checked = this.CurrentPage.IsLocked;

        //show options for the maintenence mode
        this.maintenenceOption.Visible = this.CurrentPage.IsLocked;
        this.TabMaintanance.Checked = this.CurrentPage.IsDownForMaintenance;
        
        // TODO: Find out what this code does and see if there's a better way to do this
        //this.serveAsStartPageOption.Visible = this.CurrentPage.IsLocked && _Setup.IsRoleTemplateForRegisterUser;
        
        this.TabServeAsStartPage.Checked = this.CurrentPage.ServeAsStartPageAfterLogin.GetValueOrDefault();
    }

    private void LockPageContent()
    {
        if (this.IsOwner)
        {
            ShowAddContentPanel.Enabled = HideAddContentPanel.Enabled = ChangePageTitleLinkButton.Enabled = !this.CurrentPage.IsLocked;
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

    private void LoadAddStuff()
    {
        this.WidgetListControlAdd.LoadWidgetList(newWidget =>
        {
            _OnNewWidgetCallback(newWidget);
        });
    }

    private void LoadOptionsForTemplateUser()
    {
        if (IsTemplateUser)
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
                var newCurrentPage = facade.DeletePage(this.CurrentPage.ID);
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

        if (this.AddContentPanel.Visible)
            ScriptManager.RegisterStartupScript(this.AddContentPanel, typeof(Panel), "ShowAddContentPanel" + DateTime.Now.Ticks.ToString(),
                "DropthingsUI.showWidgetGallery();", true);
    }

}