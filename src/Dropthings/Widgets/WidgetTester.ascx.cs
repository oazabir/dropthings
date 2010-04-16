#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Dropthings.Widget.Framework;
using Dropthings.Widget.Widgets;

public partial class Widgets_WidgetTester : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;

    #endregion Fields

    #region Properties

    public IWidgetHost Host
    {
        get { return _Host; }
        set { _Host = value; }
    }

    #endregion Properties

    #region Methods

    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    void IWidget.Closed()
    {
        Message.Text += "Closed. ";
    }

    void IWidget.Collasped()
    {
        Message.Text += "Maximized. ";
    }

    void IWidget.Expanded()
    {
        Message.Text += "Minimized. ";
    }

    void IWidget.HideSettings(bool userClicked)
    {
        SettingsPanel.Visible = false;
        Message.Text += "Hide Settings panel. ";
    }

    void IWidget.Init(IWidgetHost host)
    {
        this.Host = host;
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
        Message.Text += "Show Settings panel. ";
    }

    protected void CloseSettings_Click( object sender, EventArgs e )
    {
        Message.Text += "CloseSettings Clicked. ";
        (this as IWidget).HideSettings(true);
    }

    protected void CloseWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "CloseWidget Clicked";
        this.Host.Close();
    }

    protected void CollapseWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "CollapseWidget Clicked. ";
        this.Host.Collaspe();
    }

    protected void ExpandWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "ExpandWidget Clicked. ";
        this.Host.Expand();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if( !Page.IsPostBack )
        {
            Message.Text += "First Load. ";
            Data.Text = this.Host.GetState();
        }
    }

    protected void Save_Click( object sender, EventArgs e )
    {
        Message.Text += "State Saved";

        this.Host.SaveState( Data.Text );
    }

    protected void SettingsButton_Click( object sender, EventArgs e )
    {
        Message.Text += "Settings Button Clicked. ";
    }

    #endregion Methods
}