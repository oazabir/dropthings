#region Header

// Copyright (c) Mohammad Iftekharul Anam. All rights reserved.

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

using Dimebrain.TweetSharp;
using Dimebrain.TweetSharp.Fluent;


public partial class TwitterWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields
    
    private IWidgetHost _Host;
    private XElement _State;

    #endregion

    #region Properties

    private string Username
    {
        get { return State.Element("username").Value; }
        set { State.Element("username").Value = value; }
    }

    private string Password
    {
        get { return State.Element("password").Value; }
        set { State.Element("password").Value = value; }
    }

    private XElement State
    {
        get
        {
            if (IsStateEmpty())
            {
                string stateXml = this._Host.GetState();
                GetStateFromXml(stateXml);
            }
            return _State;
        }
    }

    private void GetStateFromXml(string stateXml)
    {
        if (string.IsNullOrEmpty(stateXml))
        {
            _State = new XElement("state",
                new XElement("username", ""),
                new XElement("password", ""));
        }
        else
        {
            _State = XElement.Parse(stateXml);
        }
    }

    private bool IsStateEmpty()
    {
        return _State == null;
    }

    #endregion

    #region Methods

    void IWidget.Closed()
    {
    }

    void IWidget.Collasped()
    {
    }

    void IWidget.Expanded()
    {
    }

    void IWidget.HideSettings()
    {
        this.settingsPanel.Visible = false;
        this.TwitterWidgetMultiview.Visible = true;
        this.LoadWidget(this, null);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsAsync)
        {
            this.LoadWidget(this, e);
        }
    }

    protected void LoadWidget(object sender, EventArgs e)
    {
        TwitterWidgetTimer.Enabled = false;
        if (HasUserCredential() && this.VerifyCredentials(Username, Password))
        {
            this.GetUserStatusList();
        }
        else
        {
            this.GetPublicStatuses();
        }
        this.TwitterWidgetMultiview.ActiveViewIndex = 1;
    }

    private void EnableTabs()
    {
        lbtnArchive.Enabled = true;
        lbtnFriends.Enabled = true;
        lbtnUpdate.Enabled = true;
    }

    private bool HasUserCredential()
    {
        return !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password);
    }

    private void GetUserStatusList()
    {
        TwitterService svc = new TwitterService();
        hdnResponseField.Value = svc.GetUserStatuses(Username, Password);
        EnableTabs();
        UpdateResult(true, lbtnArchive.ClientID);
    }

    private void GetPublicStatuses()
    {
        TwitterService svc = new TwitterService();
        hdnResponseField.Value = svc.GetPublicStatuses();
        if(!lbtnArchive.Enabled)
            UpdateResult(false, lbtnPublic.ClientID);
        else
            UpdateResult(true, lbtnPublic.ClientID);
    }

    private void GetFriendStatuses()
    {
        TwitterService svc = new TwitterService();
        hdnResponseField.Value = svc.GetFriendStatuses(Username, Password);
        UpdateResult(true, lbtnFriends.ClientID);
    }

    private void UpdateUserStatus()
    {
        TwitterService svc = new TwitterService();
        string response = svc.UpdateStaus(Username, Password, txtTwUpdate.Text);
        if (response.Contains(txtTwUpdate.Text))
        {
            GetUserStatusList();
        }
        else
        {
            lblTwUpdtaeError.Text = "Update was not successful. Please check your credentials.";
        }
    }

    protected void UpdateResult(bool hasCredential, string tabClientID)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "TwitterResults", "if(typeof(Tw_op) == 'undefined') var Tw_op = new TwitterWidgetOperations();Tw_op.LoggedIn('" + hasCredential + "');Tw_op.SetSelected('" + tabClientID + "');Tw_op.SetDisplayFromResponse();", true);
    }

    private void ShowStatusPanel()
    {
    }

    private void HideStatusPanel()
    {
    }

    public void ShowSettings()
    {
        this.TwitterWidgetMultiview.Visible = false;
        this.settingsPanel.Visible = true;
        LoadSettingsFromState();
    }

    private void LoadSettingsFromState()
    {
        txtTwUsername.Text = Username;
        txtTwPassword.Text = Password;
    }
 
    public void AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (VerifyCredentials(txtTwUsername.Text, txtTwPassword.Text))
        {
            Username = txtTwUsername.Text;
            Password = txtTwPassword.Text;
            this._Host.SaveState(this.State.Xml());
            (this as IWidget).HideSettings();
        }
        else
            litTwError.Text = "Could not authenticate you";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LoadSettingsFromState();
        (this as IWidget).HideSettings();
    }

    private bool VerifyCredentials(string username, string password)
    {
        TwitterService svc = new TwitterService();
        hdnResponseField.Value = svc.VerifyCredentials(username, password);
        if (hdnResponseField.Value.Contains("Could not authenticate you"))
            return false;
        else
            return true;
    }

    protected void lbtnPublic_Click(object sender, EventArgs e)
    {
        GetPublicStatuses();
    }

    protected void lbtnFriends_Click(object sender, EventArgs e)
    {
        GetFriendStatuses();
    }

    protected void lbtnArchive_Click(object sender, EventArgs e)
    {
        GetUserStatusList();
    }

    protected void btnTwUpdate_Click(object sender, EventArgs e)
    {
        UpdateUserStatus();
    }

    protected void btnTwShowSettings_Click(object sender, EventArgs e)
    {
        ShowSettings();
    }

    #endregion
}
