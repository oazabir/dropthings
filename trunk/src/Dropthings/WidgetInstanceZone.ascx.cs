using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.DataAccess;
using Dropthings.Widget.Framework;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows.WidgetWorkflows;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows;

public partial class WidgetInstanceZone : System.Web.UI.UserControl
{
    private const string ZONE_ID_ATTR = "_zoneid";
    public List<WidgetInstance> WidgetInstances { get; set; }

    public int Width { get; set; }
    public string WidgetZoneClass { get; set; }
    public string WidgetClass { get; set; }
    public string NewWidgetClass { get; set; }
    public int WidgetZoneId { get; set; }
    public string WidgetContainerPath { get; set; }
    public string UserName { get; set; }
    public string HandleClass { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        this.WidgetHolderPanel.CssClass = this.WidgetZoneClass;

        //ScriptManager.RegisterStartupScript(this, typeof(WidgetInstanceZone), this.ClientID + "_InitWidgets",
        //        "$(document).ready(function() {" + "DropthingsUI.initWidgetActions('{0}', '{1}'); DropthingsUI.initDragDrop('{0}', '{1}', '{2}', '{3}', '{4}'); DropthingsUI.initAddStuff('{4}', '{2}');"
        //        .FormatWith(this.WidgetHolderPanel.ClientID, this.WidgetClass, this.NewWidgetClass, this.HandleClass, this.WidgetZoneClass) + "});", true);

        ScriptManager.RegisterStartupScript(this.WidgetHolderPanel, typeof(Panel), this.WidgetHolderPanel.ClientID + "_InitWidgets",
                "$(document).ready(function() {" + "DropthingsUI.initWidgetActions('{0}', '{1}'); DropthingsUI.initDragDrop('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');"
                .FormatWith(this.WidgetHolderPanel.ClientID, this.WidgetClass, this.NewWidgetClass, this.HandleClass, this.WidgetZoneClass, WidgetHolderPanelTrigger.ClientID) + "});", true);

        base.OnPreRender(e);
    }

    public void LoadWidgets()
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

        foreach (WidgetInstance instance in this.WidgetInstances)
        {
            var widget = LoadControl(this.WidgetContainerPath) as Control;
            widget.ID = "WidgetContainer" + instance.Id.ToString();
            var widgetHost = widget as IWidgetHost;
            widgetHost.WidgetInstance = instance;

            widgetHost.Deleted += new Action<WidgetInstance, IWidgetHost>(Widget_Deleted);

            this.WidgetHolderPanel.Controls.Add(widget);
        }

    }

    private void Widget_Deleted(WidgetInstance wi, IWidgetHost host)
    {
        if (host is Control)
            this.WidgetHolderPanel.Controls.Remove(host as Control);
        this.RefreshZoneUpdatePanel();
    }

    private void RefreshZoneUpdatePanel()
    {
        this.WidgetZoneUpdatePanel.Update();

        //ScriptManager.RegisterStartupScript(this.WidgetHolderPanelTrigger, this.WidgetHolderPanelTrigger.GetType(), this.ClientID + "_InitWidgets" + DateTime.Now.Ticks.ToString(),
        //        "$(document).ready(function() {" + "DropthingsUI.refreshSortable('{0}');"
        //        .FormatWith(this.WidgetPanel.ClientID) + "});", true);
    }

    public void Refresh()
    {
        this.RefreshZoneUpdatePanel();
    }

    protected void WidgetHolderPanelTrigger_Click(object sender, EventArgs e)
    {
        this.RefreshZoneUpdatePanel();
    }

}
