using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Workflow.Runtime;

using Dropthings.Business;
using Dropthings.DataAccess;
using Dropthings.Web.Framework;
using Dropthings.Web.Util;

using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;

public partial class WidgetListControl : System.Web.UI.UserControl
{
    #region Fields

    private WidgetPanelRefreshHandler widgetRefreshCallback;

    #endregion Fields

    #region Delegates

    public delegate void WidgetPanelRefreshHandler(WidgetInstance WI);

    #endregion Delegates

    #region Properties

    private int AddStuffPageIndex
    {
        get { object val = ViewState["AddStuffPageIndex"]; if (val == null) return 0; else return (int)val; }
        set { ViewState["AddStuffPageIndex"] = value; }
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
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                        widgets = facade.GetWidgetList(Profile.UserName, Enumerations.WidgetTypeEnum.PersonalPage);
                    //       Cache["Widgets"] = widgets;
                }
                return widgets;
            }
            else
            {
                List<Widget> widgets = Cache["Widgets"] as List<Widget>;
                if (null == widgets)
                {
                    using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
                        widgets = facade.GetWidgetList(Enumerations.WidgetTypeEnum.PersonalPage);
                    Cache["Widgets"] = widgets;
                }
                return widgets;
            }
        }
    }

    #endregion Properties

    #region Methods

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

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void WidgetListNextButton_Click(object sender, EventArgs e)
    {
        AddStuffPageIndex++;
        this.LoadWidgetList();
    }

    protected void WidgetListPreviousLinkButton_Click(object sender, EventArgs e)
    {
        if (0 == AddStuffPageIndex) return;
        AddStuffPageIndex--;

        this.LoadWidgetList();
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

        //DashboardFacade facade = new DashboardFacade(HttpContext.Current.Profile.UserName);

        //User added a new widget. The new widget is loaded for the first time. So, it's not
        //a postback experience for the widget. But for rest of the widgets, it is a postback experience.
        WidgetInstance widgetInstance;
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            widgetInstance = facade.AddWidget(widgetId, 0, 0, 0);
        }

        // -- Workflow way. Obselete.
        //var response = WorkflowHelper.Run<AddWidgetWorkflow,AddWidgetRequest,AddWidgetResponse>(
        //                   new AddWidgetRequest { WidgetId = widgetId, RowNo = 0, ColumnNo = 0, ZoneId = 0, UserName = Profile.UserName }
        //               );

        //widgetRefreshCallback(response.WidgetInstanceAffected);
        widgetRefreshCallback(widgetInstance);
    }

    #endregion Methods
}