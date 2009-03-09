using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.DataAccess;
using Dropthings.Business;
using Dropthings.Business.Workflows;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows.TabWorkflows;
using System.Workflow.Runtime;
using System.Web.UI.HtmlControls;
using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

public partial class WidgetPage : System.Web.UI.UserControl
{
    public event EventHandler OnReloadPage;

    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";
    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    public Dropthings.DataAccess.Page CurrentPage { get; set; }
    public List<WidgetInstance> WidgetInstances { get; set; }
    private EventBrokerService _EventBroker = new EventBrokerService();    

    public void LoadWidgets(Dropthings.DataAccess.Page page, string widgetContainerPath)
    {
        this.CurrentPage = page;
    
        this.SetupWidgetZones(widgetContainerPath);
    }

    private void SetupWidgetZones(string widgetContainerPath)    
    {
        this.Controls.Clear();

        var maxBackground = new HtmlGenericControl("div");
        maxBackground.ID = "widgetMaxBackground";
        maxBackground.Attributes.Add("class", "widget_max_holder");
        maxBackground.Style.Add("display", "none");
        this.Controls.Add(maxBackground);

        var columns = RunWorkflow.Run<GetColumnsInPageWorkflow, GetColumnsInPageWorkflowRequest, GetColumnsInPageWorkflowResponse>(
                new GetColumnsInPageWorkflowRequest { PageId = this.CurrentPage.ID, UserName = Profile.UserName }
            ).Columns;
            
        columns.Each<Column>( (column, index) =>
        {
            var widgetZone = LoadControl("~/WidgetInstanceZone.ascx") as WidgetInstanceZone;
            widgetZone.ID = "WidgetZone" + column.WidgetZoneId;
            widgetZone.WidgetZoneId = column.WidgetZoneId;
            widgetZone.WidgetContainerPath = widgetContainerPath;
            widgetZone.WidgetZoneClass = "widget_zone";
            widgetZone.WidgetClass = "widget";
            widgetZone.NewWidgetClass = "new_widget";
            widgetZone.HandleClass = "widget_header";
            widgetZone.UserName = Profile.UserName;

            var panel = new Panel();
            panel.ID = "WidgetZonePanel" + index;
            panel.CssClass = "column";
            panel.Style.Add(HtmlTextWriterStyle.Width, column.ColumnWidth.Percent());
            
            this.Controls.Add(panel);
            panel.Controls.Add(widgetZone);           
 
            widgetZone.LoadWidgets(this._EventBroker);
        });
    }
    
    public void RefreshZone(int widgetZoneId)
    {
        var widgetZone = this.FindControl("WidgetZone" + widgetZoneId) as WidgetInstanceZone;
        widgetZone.Refresh();
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

}
