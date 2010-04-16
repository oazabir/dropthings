using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;
using Dropthings.Business.Facade;
using Dropthings.Business.Facade.Context;
using Dropthings.Web.Util;

using Dropthings.DataAccess;
using System.Collections.Generic;

public partial class Widgets_MemberInRoleWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;
    private XElement _State;
    private bool _hasProcessedHeader;

    #endregion Fields

    #region Properties

    private XElement State
    {
        get
        {
            string state = this._Host.GetState();
            if (string.IsNullOrEmpty(state))
                state = "<state></state>";
            if (_State == null) _State = XElement.Parse(state);
            return _State;
        }
    }

    public string CurrentMemberUsername
    {
        get { object val = ViewState[this.ClientID + "_CurrentMemberUsername"]; if (val == null) return string.Empty; else return (val.ToString()); }
        set { ViewState[this.ClientID + "_CurrentMemberUsername"] = value; }
    }

    #endregion Properties

    #region Methods

    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    void IWidget.Closed()
    {
    }

    void IWidget.Collasped()
    {
    }

    void IWidget.Expanded()
    {
    }

    void IWidget.HideSettings(bool userClicked)
    {
        SettingsPanel.Visible = false;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.Maximized()
    {
    }

    void IWidget.Restored()
    {
    }

    void IWidget.ShowSettings(bool userClicked)
    {
        SettingsPanel.Visible = true;
    }

    private MembershipUser _currentMember;
    private MembershipUser CurrentMember
    {
        get
        {
            if (!string.IsNullOrEmpty(CurrentMemberUsername))
            {
                using (var facade = new Facade(AppContext.GetContext(Context)))
                {
                    _currentMember = facade.GetUser(CurrentMemberUsername);
                }
            }

            return _currentMember;
        }
    }

    private void PopulateRoles()
    {
        ddlRole.Items.Clear();

        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            List<aspnet_Role> roles = facade.GetAllRole();

            foreach (aspnet_Role role in roles)
            {
                ddlRole.Items.Add(role.RoleName);
            }
        }
    }

    private void PrepareMemberEditor()
    {
        MembershipUser member = CurrentMember;
        
        if (member != null)
        {
            txtUsername.Text = member.UserName;
            txtUsername.Enabled = false;
            txtEmail.Text = member.Email;
            chkIsActive.Checked = member.IsApproved;
        }
    }

    private void SaveMember()
    {
        MembershipUser member = CurrentMember;

        if (member != null)
        {
            member.Email = txtEmail.Text;
            member.IsApproved = chkIsActive.Checked;

            using (var facade = new Facade(AppContext.GetContext(Context)))
            {
                facade.UpdateUser(member);
            }
        }
        else
        {
            using (var facade = new Facade(AppContext.GetContext(Context)))
            {
                MembershipCreateStatus status;
                facade.CreateUser(txtUsername.Text.Trim(), txtPassword.Text.Trim(), txtEmail.Text.Trim(), chkIsActive.Checked, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(txtUsername.Text.Trim(), ddlRole.SelectedValue);
                    WebUtil.ShowMessage(lblMessage, "Member has been successfully added.", false);
                    BindList();            
                }
                else
                {
                    switch (status)
                    {
                        case MembershipCreateStatus.DuplicateUserName:
                            {
                                WebUtil.ShowMessage(lblMessage, "User name already exists.", true);
                                break;
                            }
                        case MembershipCreateStatus.DuplicateEmail:
                            {
                                WebUtil.ShowMessage(lblMessage, "Email already exists.", true);
                                break;
                            }
                        case MembershipCreateStatus.InvalidUserName:
                            {
                                WebUtil.ShowMessage(lblMessage, "Invalid user name.", true);
                                break;
                            }
                        case MembershipCreateStatus.InvalidPassword:
                            {
                                WebUtil.ShowMessage(lblMessage, "Invalid password. Password must be atleast 6 character.", true);
                                break;
                            }
                    }
                }
            }
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            PopulateRoles();
        }
    }

    protected void SaveClicked(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveMember();
        }
    }

    protected void ddlRole_SelectionChanged(object sender, EventArgs e)
    {
        BindList();
    }

    protected void CancelClicked(object sender, EventArgs e)
    {
        BindList();
    }

    protected void AddNewMemberClicked(object sender, EventArgs e)
    {
        this.CurrentMemberUsername = string.Empty;
        PrepareMemberEditor();
        this.Multiview.ActiveViewIndex = 1;
    }

    protected void ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        string userName = e.CommandArgument.ToString();

        if (!string.IsNullOrEmpty(userName))
        {
            if (string.Equals(e.CommandName, "DeleteItem"))
            {
                using (var facade = new Facade(AppContext.GetContext(Context)))
                {
                    facade.DeleteUser(userName);
                    BindList();
                    WebUtil.ShowMessage(lblMessage, "Member successfully deleted.", false);
                }
            }
        }
    }

    public void BindList()
    {
        this.lvItems.DataBind();
        this.Multiview.ActiveViewIndex = 0;
    }

    #endregion Methods
}