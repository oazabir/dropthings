using System;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;
using System.Linq;
using System.Data.Linq;
using Dropthings.Business;
using Dropthings.DataAccess;
using Dropthings.Web.Util;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.WidgetWorkflows;
using System.Workflow.Runtime;
using System.Globalization;
using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;

public partial class WidgetListControl : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private List<Widget> WidgetList
    {
        get
        {
            if (Roles.Enabled && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableWidgetPermission"], CultureInfo.InvariantCulture))
            {
                List<Widget> widgets;// = Cache["Widgets"] as List<Widget>;
                //   if (null == widgets)
                {
                    widgets = new DashboardFacade(Profile.UserName).GetWidgetList(Profile.UserName, Enumerations.WidgetTypeEnum.PersonalPage);
                    //       Cache["Widgets"] = widgets;
                }
                return widgets;
            }
            else
            {
                List<Widget> widgets = Cache["Widgets"] as List<Widget>;
                if (null == widgets)
                {
                    widgets = new DashboardFacade(Profile.UserName).GetWidgetList(Enumerations.WidgetTypeEnum.PersonalPage);
                    Cache["Widgets"] = widgets;
                }
                return widgets;
            }
        }
    }

    public delegate void WidgetPanelRefreshHandler(WidgetInstance WI);
    private WidgetPanelRefreshHandler widgetRefreshCallback;

    public void LoadWidgetList(WidgetPanelRefreshHandler widgetAddedCallback)
    {
        this.LoadWidgetList();

        widgetRefreshCallback = widgetAddedCallback;
    }

    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(WidgetListControl), this.ID + "_initAddStuff",
                "DropthingsUI.initAddStuff('widget_zone', 'new_widget');"
                , true);

        base.OnPreRender(e);
    }
    
    private void LoadWidgetList()
    {
        this.WidgetDataList.ItemCommand += new DataListCommandEventHandler(WidgetDataList_ItemCommand);

        var itemsToShow = WidgetList.Skip(AddStuffPageIndex * 30).Take(30);
        this.WidgetDataList.DataSource = itemsToShow;
        this.WidgetDataList.DataBind();

        WidgetListPreviousLinkButton.Visible = AddStuffPageIndex > 0;
        WidgetListNextButton.Visible = AddStuffPageIndex * 30 + 30 < WidgetList.Count;
    }

    void WidgetDataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.AddNewWidget)) return;

        int widgetId = int.Parse(e.CommandArgument.ToString());

        DashboardFacade facade = new DashboardFacade(HttpContext.Current.Profile.UserName);
        
        //User added a new widget. The new widget is loaded for the first time. So, it's not 
        //a postback experience for the widget. But for rest of the widgets, it is a postback experience.
        var response = RunWorkflow.Run<AddWidgetWorkflow,AddWidgetRequest,AddWidgetResponse>(
                           new AddWidgetRequest { WidgetId = widgetId, RowNo = 0, ColumnNo = 0, ZoneId = 0, UserName = Profile.UserName }
                       );

        widgetRefreshCallback(response.NewWidget);
    }

    protected void WidgetListPreviousLinkButton_Click(object sender, EventArgs e)
    {
        if (0 == AddStuffPageIndex) return;
        AddStuffPageIndex--;

        this.LoadWidgetList();
    }
    protected void WidgetListNextButton_Click(object sender, EventArgs e)
    {
        AddStuffPageIndex++;
        this.LoadWidgetList();
    }

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if (val == null) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
    }
}
 