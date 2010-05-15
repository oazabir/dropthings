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

        using (var facade = new Facade(AppContext.GetContext(Context)))
        {
            this.WidgetInstances = facade.GetWidgetInstancesInZoneWithWidget(WidgetZoneId);
        }

        var controlsToDelete = new List<Control>();
        foreach (Control control in this.WidgetZoneUpdatePanel.ContentTemplateContainer.Controls)
            if (control is IWidgetHost)
                controlsToDelete.Add(control);
        controlsToDelete.ForEach((c) => this.WidgetZoneUpdatePanel.ContentTemplateContainer.Controls.Remove(c));

        List<IWidgetHost> widgetHosts = new List<IWidgetHost>();
        this.WidgetInstances.Each(instance =>
        {
            var widget = LoadControl(this.WidgetContainerPath) as Control;
            widget.ID = "WidgetContainer" + instance.Id.ToString();

            var widgetHost = widget as IWidgetHost;
            widgetHost.WidgetInstance = instance;
            widgetHost.IsLocked = this.IsLocked;
            widgetHost.EventBroker = eventBroker;

            widgetHost.Deleted += new Action<WidgetInstance, IWidgetHost>(Widget_Deleted);

            this.WidgetHolderPanel.Controls.Add(widget);

            widgetHosts.Add(widgetHost);
        });

        return widgetHosts;
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