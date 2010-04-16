using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Dropthings.Widget.Framework;

public partial class Widgets_EventTest_Publisher : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;

    #endregion Fields

    #region Methods

    public void AcceptEvent(object sender, EventArgs e)
    {
        if (sender != this && e is MasterChildEventArgs)
        {
            var arg = e as MasterChildEventArgs;
            this.Received.Text = arg.Who + " says, " + arg.Message;
            _Host.Refresh(this);
        }
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
    }

    void IWidget.Init(IWidgetHost host)
    {
        _Host = host;
        host.EventBroker.AddListener(this);
    }

    void IWidget.Maximized()
    {
    }

    void IWidget.Restored()
    {
    }

    void IWidget.ShowSettings(bool userClicked)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Raise_Clicked(object sender, EventArgs e)
    {
        MasterChildEventArgs args = new MasterChildEventArgs("Master " + _Host.WidgetInstance.Id, this.Message.Text);
        _Host.EventBroker.RaiseEvent(this, args);
    }

    #endregion Methods
}