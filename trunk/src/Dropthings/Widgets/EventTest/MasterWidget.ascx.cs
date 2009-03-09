using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Widget.Framework;

public partial class Widgets_EventTest_Publisher : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region IWidget Members

    void IWidget.Init(IWidgetHost host)
    {
        _Host = host;
        host.EventBroker.AddListener(this);
    }

    void IWidget.ShowSettings()
    {
        
    }

    void IWidget.HideSettings()
    {
        
    }

    void IWidget.Expanded()
    {
        
    }

    void IWidget.Collasped()
    {
        
    }

    void IWidget.Maximized()
    {
        
    }

    void IWidget.Restored()
    {
        
    }

    void IWidget.Closed()
    {
        
    }

    #endregion

    #region IEventListener Members

    public void AcceptEvent(object sender, EventArgs e)
    {
        if (sender != this && e is MasterChildEventArgs)
        {
            var arg = e as MasterChildEventArgs;
            this.Received.Text = arg.Who + " says, " + arg.Message;
            _Host.Refresh(this);
        }
    }

    #endregion

    protected void Raise_Clicked(object sender, EventArgs e)
    {
        MasterChildEventArgs args = new MasterChildEventArgs("Master " + _Host.WidgetInstance.Id, this.Message.Text);
        _Host.EventBroker.RaiseEvent(this, args);
    }
}
