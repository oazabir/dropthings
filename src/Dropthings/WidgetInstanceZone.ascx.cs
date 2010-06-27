using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;

using Dropthings.Data;
using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;
using Dropthings.Util;

public partial class WidgetInstanceZone : System.Web.UI.UserControl
{
    #region Fields

    public const string WIDGET_ZONE_ID_PREFIX = "WidgetZone";

    private const string ZONE_ID_ATTR = "_zoneid";

    #endregion Fields

    #region Properties

    public string HandleClass
    {
        get; set;
    }

    public string NewWidgetClass
    {
        get; set;
    }

    public string UserName
    {
        get; set;
    }

    public string WidgetClass
    {
        get; set;
    }

    public string WidgetContainerPath
    {
        get; set;
    }

    public IEnumerable<WidgetInstance> WidgetInstances
    {
        get; set;
    }

    public string WidgetZoneClass
    {
        get; set;
    }

    public int WidgetZoneId
    {
        get; set;
    }

    public int Width
    {
        get; set;
    }

    public bool IsLocked
    {
        get; set;
    }

    #endregion Properties

    #region Methods

    public List<IWidgetHost> LoadWidgets(EventBrokerService eventBroker)
    {
        this.WidgetHolderPanel.Attributes.Add(ZONE_ID_ATTR, this.WidgetZoneId.ToString());
        //this.WidgetHolderPanelTrigger.CssClass = "WidgetZoneUpdatePanel_" + this.WidgetZoneId.ToString();

        var facade = Services.Get<Facade>();
        {
            this.WidgetInstances = facade.GetWidgetInstancesInZoneWithWidget(WidgetZoneId);
        }

        var controlsToDelete = new List<Control>();
        foreach (Control control in this.WidgetHolderPanel.Controls)
            if (control is IWidgetHost)
                controlsToDelete.Add(control);
        controlsToDelete.ForEach((c) => this.WidgetHolderPanel.Controls.Remove(c));

        List<IWidgetHost> widgetHosts = new List<IWidgetHost>();
        this.WidgetInstances.Each(instance =>
        {
            var widgetHost = CreateWidgetContainerFromWidgetInstance(eventBroker, instance);
            widgetHosts.Add(widgetHost);

            this.WidgetHolderPanel.Controls.Add(widgetHost as Control);        
        });

        return widgetHosts;
    }

    private IWidgetHost CreateWidgetContainerFromWidgetInstance(EventBrokerService eventBroker, WidgetInstance instance)
    {
        var widget = LoadControl(this.WidgetContainerPath) as Control;
        widget.ID = "WidgetContainer" + instance.Id.ToString();

        var widgetHost = widget as IWidgetHost;
        widgetHost.WidgetInstance = instance;
        widgetHost.IsLocked = this.IsLocked;
        widgetHost.EventBroker = eventBroker;

        widgetHost.Deleted += new Action<WidgetInstance, IWidgetHost>(Widget_Deleted);

        return widgetHost;
    }

    public void AddNewWidget(EventBrokerService eventBroker, WidgetInstance instance)
    {
        var widgetHost = CreateWidgetContainerFromWidgetInstance(eventBroker, instance);
        var widgetContainer = widgetHost as Control;

        var existingControls = this.WidgetHolderPanel.Controls;
        if (existingControls == null || existingControls.Count == 0)
        {
            this.WidgetHolderPanel.Controls.Add(widgetContainer);
        }
        else
        {
            var position = 0;
            foreach (Control existingControl in existingControls)
            {
                if (existingControl is IWidgetHost)
                {
                    var existingHost = existingControl as IWidgetHost;
                    if (existingHost.WidgetInstance.OrderNo >= instance.OrderNo)
                    {
                        break;
                    }
                }

                
            }
            existingControls.AddAt(position, widgetContainer);
        }

        this.Refresh();
    }

    public void Refresh()
    {
        this.RefreshZoneUpdatePanel();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.WidgetHolderPanel.CssClass = this.WidgetZoneClass;

        ScriptManager.RegisterStartupScript(this.WidgetHolderPanel, typeof(Panel), this.WidgetHolderPanel.ClientID + "_InitWidgets",
            (Page.IsPostBack ? "" : " jQuery(document).ready(function() {") 
                + "DropthingsUI.initDragDrop('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');"
                    .FormatWith(this.WidgetHolderPanel.ClientID, this.WidgetClass, this.NewWidgetClass, this.HandleClass, this.WidgetZoneClass, WidgetHolderPanelTrigger.ClientID) 
            + (Page.IsPostBack ? "" : "});"), true);

        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void WidgetHolderPanelTrigger_Click(object sender, EventArgs e)
    {
        this.RefreshZoneUpdatePanel();
    }

    private void RefreshZoneUpdatePanel()
    {
        this.WidgetZoneUpdatePanel.Update();
    }

    private void Widget_Deleted(WidgetInstance wi, IWidgetHost host)
    {
        if (host is Control)
            this.WidgetHolderPanel.Controls.Remove(host as Control);
        this.RefreshZoneUpdatePanel();
    }

    #endregion Methods
}