using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Workflow.Runtime;

using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.WidgetWorkflows;
using Dropthings.DataAccess;
using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

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

    public List<WidgetInstance> WidgetInstances
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

    #endregion Properties

    #region Methods

    public List<IWidgetHost> LoadWidgets(EventBrokerService eventBroker)
    {
        this.WidgetHolderPanel.Attributes.Add(ZONE_ID_ATTR, this.WidgetZoneId.ToString());
        //this.WidgetHolderPanelTrigger.CssClass = "WidgetZoneUpdatePanel_" + this.WidgetZoneId.ToString();

        var response = RunWorkflow.Run<LoadWidgetInstancesInZoneWorkflow, LoadWidgetInstancesInZoneRequest, LoadWidgetInstancesInZoneResponse>(
                        new LoadWidgetInstancesInZoneRequest { WidgetZoneId = this.WidgetZoneId, UserName = this.UserName }
                    );

        this.WidgetInstances = response.WidgetInstances;

        var controlsToDelete = new List<Control>();
        foreach (Control control in this.WidgetZoneUpdatePanel.ContentTemplateContainer.Controls)
            if (control is IWidgetHost)
                controlsToDelete.Add(control);
        controlsToDelete.ForEach((c) => this.WidgetZoneUpdatePanel.ContentTemplateContainer.Controls.Remove(c));

        List<IWidgetHost> widgetHosts = new List<IWidgetHost>();
        foreach (WidgetInstance instance in this.WidgetInstances)
        {
            var widget = LoadControl(this.WidgetContainerPath) as Control;
            widget.ID = "WidgetContainer" + instance.Id.ToString();
            var widgetHost = widget as IWidgetHost;
            widgetHost.WidgetInstance = instance;
            widgetHost.EventBroker = eventBroker;

            widgetHost.Deleted += new Action<WidgetInstance, IWidgetHost>(Widget_Deleted);

            this.WidgetHolderPanel.Controls.Add(widget);

            widgetHosts.Add(widgetHost);
        }

        return widgetHosts;
    }

    public void Refresh()
    {
        this.RefreshZoneUpdatePanel();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.WidgetHolderPanel.CssClass = this.WidgetZoneClass;

        //ScriptManager.RegisterStartupScript(this, typeof(WidgetInstanceZone), this.ClientID + "_InitWidgets",
        //        "$(document).ready(function() {" + "DropthingsUI.initWidgetActions('{0}', '{1}'); DropthingsUI.initDragDrop('{0}', '{1}', '{2}', '{3}', '{4}'); DropthingsUI.initAddStuff('{4}', '{2}');"
        //        .FormatWith(this.WidgetHolderPanel.ClientID, this.WidgetClass, this.NewWidgetClass, this.HandleClass, this.WidgetZoneClass) + "});", true);

        ScriptManager.RegisterStartupScript(this.WidgetHolderPanel, typeof(Panel), this.WidgetHolderPanel.ClientID + "_InitWidgets",
                "$(document).ready(function() {" + " /*DropthingsUI.initWidgetActions('{0}', '{1}');*/ DropthingsUI.initDragDrop('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');"
                .FormatWith(this.WidgetHolderPanel.ClientID, this.WidgetClass, this.NewWidgetClass, this.HandleClass, this.WidgetZoneClass, WidgetHolderPanelTrigger.ClientID) + "});", true);

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

        //ScriptManager.RegisterStartupScript(this.WidgetHolderPanelTrigger, this.WidgetHolderPanelTrigger.GetType(), this.ClientID + "_InitWidgets" + DateTime.Now.Ticks.ToString(),
        //        "$(document).ready(function() {" + "DropthingsUI.refreshSortable('{0}');"
        //        .FormatWith(this.WidgetPanel.ClientID) + "});", true);
    }

    private void Widget_Deleted(WidgetInstance wi, IWidgetHost host)
    {
        if (host is Control)
            this.WidgetHolderPanel.Controls.Remove(host as Control);
        this.RefreshZoneUpdatePanel();
    }

    #endregion Methods
}