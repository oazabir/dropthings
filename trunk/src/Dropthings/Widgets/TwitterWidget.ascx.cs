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
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Xml;
using Dropthings.Web.Util;
using Dropthings.Util;


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

    public string WidgetID
    {
        get
        {
            return this.ID;
        }
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

    public string WidgetState
    {
        get
        {
            return State.Xml();
        }
    }

    public int WidgetHostID
    {
        get { return this._Host.ID; }
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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        var scriptToLoad = ResolveClientUrl("~/Widgets/TwitterWidget/TwitterService.js?v=" + ConstantHelper.ScriptVersionNo);
        var startUpCode = string.Format("var tw = new Twitter(); tw.load({0});", WidgetHostID);

        WidgetHelper.RegisterWidgetScript(this, scriptToLoad, startUpCode);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void LoadWidget(object sender, EventArgs e)
    {
        
    }

    private void ShowStatusPanel()
    {
    }

    private void HideStatusPanel()
    {
    }
 
    public void AcceptEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region IWidget Members

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

    public void ShowSettings(bool userClicked)
    {
        //throw new NotImplementedException();
    }

    #endregion
}
