// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Dropthings.Widget.Widgets;
using Dropthings.Widget.Framework;


public partial class Widgets_WidgetTester : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;

    public IWidgetHost Host
    {
        get { return _Host; }
        set { _Host = value; }
    }
	
    protected void Page_Load(object sender, EventArgs e)
    {
        if( !Page.IsPostBack )
        {
            Message.Text += "First Load. ";
            Data.Text = this.Host.GetState();
        }
    }

    void IWidget.Init(IWidgetHost host)
    {
        this.Host = host;
    }

    void IWidget.ShowSettings()
    {
        SettingsPanel.Visible = true;
        Message.Text += "Show Settings panel. ";
    }
    void IWidget.HideSettings()
    {
        SettingsPanel.Visible = false;
        Message.Text += "Hide Settings panel. ";
    }
    void IWidget.Expanded()
    {
        Message.Text += "Minimized. ";
    }
    void IWidget.Collasped()
    {
        Message.Text += "Maximized. ";
    }
    void IWidget.Closed()
    {
        Message.Text += "Closed. ";
    }

    void IWidget.Restored()
    {
    }
    void IWidget.Maximized()
    {
    }

    protected void SettingsButton_Click( object sender, EventArgs e )
    {
        Message.Text += "Settings Button Clicked. ";
    }
    protected void CloseSettings_Click( object sender, EventArgs e )
    {
        Message.Text += "CloseSettings Clicked. ";
        (this as IWidget).HideSettings();
    }
    protected void ExpandWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "ExpandWidget Clicked. ";
        this.Host.Expand();
    }
    protected void CollapseWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "CollapseWidget Clicked. ";
        this.Host.Collaspe();
    }
    protected void CloseWidget_Click( object sender, EventArgs e )
    {
        Message.Text += "CloseWidget Clicked";
        this.Host.Close();
    }
    
    protected void Save_Click( object sender, EventArgs e )
    {
        Message.Text += "State Saved";
        
        this.Host.SaveState( Data.Text );
    }


    #region IWidget Members


    void IEventListener.AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion
}
