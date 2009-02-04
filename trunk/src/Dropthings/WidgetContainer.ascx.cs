// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Dropthings.Business;
using Dropthings.Widget.Framework;
using Dropthings.DataAccess;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.WidgetWorkflows;
using System.Workflow.Runtime;

public partial class WidgetContainer : System.Web.UI.UserControl, IWidgetHost
{
    public const string ATTR_INSTANCEID = "_InstanceId";
    public const string ATTR_INSTANCE_ID = "_InstanceId";
    public const string ATTR_RESIZED = "_Resized";
    public const string ATTR_EXPANDED = "_Expanded";
    public const string ATTR_MAXIMIZED = "_Maximized";
    public const string ATTR_WIDTH = "_Width";
    public const string ATTR_HEIGHT = "_Height";

    public event Action<WidgetInstance, IWidgetHost> Deleted;

    public bool SettingsOpen
    {
        get
        {
            object val = ViewState[this.ClientID + "_SettingsOpen"] ?? false;
            return (bool)val;
        }
        set { ViewState[this.ClientID + "_SettingsOpen"] = value; }
    }

    private WidgetInstance _WidgetInstance;

    public WidgetInstance WidgetInstance
    {
        get { return _WidgetInstance; }
        set { _WidgetInstance = value; }
    }

    public Widget WidgetDef { get; set; }

    private IWidget _WidgetRef;

    private bool _IsFirstLoad;

    public bool IsFirstLoad
    {
        get { return _IsFirstLoad; }
        set { _IsFirstLoad = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (this.WidgetInstance.Resized)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "overflow: hidden; height: " + this.WidgetInstance.Height + "px");
            this.WidgetResizeFrame.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
            this.WidgetResizeFrame.Style.Add(HtmlTextWriterStyle.Height, this.WidgetInstance.Height + "px");
        }
        /*
        if (this.WidgetInstance.Maximized)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), this.ID + "maximizeWidget",
            "DropthingsUI.Actions.maximizeWidget('" + this.WidgetInstance.Id + "', '" + this.WidgetInstance.Expanded + "');", true);
        }
        */

        if (this.WidgetInstance.Expanded)
        {
            //this.WidgetResizeFrame.Visible = true;
            //if( ScriptManager.GetCurrent(Page).IsInAsyncPostBack )
                //ScriptManager.RegisterClientScriptBlock(this.WidgetResizeFrame, this.WidgetResizeFrame.GetType(), this.ID + "_initResizer_" + DateTime.Now.Ticks.ToString(),
                                //"DropthingsUI.initResizer('" + this.WidgetResizeFrame.ClientID + "');",true);
            //else
                ScriptManager.RegisterStartupScript(this.WidgetResizeFrame, this.WidgetResizeFrame.GetType(), this.ID + "_initResizer",
                    "DropthingsUI.initResizer('" + this.WidgetResizeFrame.ClientID + "');",true);
        }
        else
        {
            this.WidgetResizeFrame.Style.Add("display", "none");
            //this.WidgetResizeFrame.Visible = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WidgetTitle.Text = this.WidgetInstance.Title;
        this.SetExpandCollapseButtons();
        this.SetMaximizeRestoreButtons();
        WidgetTitleTextBox.Style.Add("display", "none");
        SaveWidgetTitle.Style.Add("display", "none");
        
        //this.CloseWidget.OnClientClick = "DeleteWarning.show( function() { __doPostBack('" + this.CloseWidget.UniqueID+ "','') }, Function.emptyFunction ); return false; ";
        //this.CloseWidget.OnClientClick = "DeleteWarning.show( function() { DropthingsUI.Actions.deleteWidget('" + this.WidgetInstance.Id + "')}, Function.emptyFunction ); return false; ";
        //this.CollapseWidget.OnClientClick = "DropthingsUI.Actions.minimizeWidget('" + this.WidgetInstance.Id + "')";
        //this.ExpandWidget.OnClientClick = "DropthingsUI.Actions.expandWidget('" + this.WidgetInstance.Id + "')";
        //this.MaximizeWidget.OnClientClick = "DropthingsUI.Actions.maximizeWidget('" + this.WidgetInstance.Id + "', 'true');";
        //this.RestoreWidget.OnClientClick = "DropthingsUI.Actions.restoreWidget('" + this.WidgetInstance.Id + "');";

        // ILIAS 1/13/2009: Turning it off because new users are not getting the "Delete Widget" icon
        //if (Roles.Enabled)
        //{
        //    if (!Roles.IsUserInRole("AddRemovePageWidgets"))
        //    {
        //        this.CloseWidget.Visible = false;
        //    }
        //}

        if (WidgetInstance.Widget.IsLocked || this.WidgetInstance.Maximized)
        {
            Widget.CssClass = "widget nodrag";
            Widget.Attributes.Add("onmouseover", "this.className='widget nodrag widget_hover'");
            Widget.Attributes.Add("onmouseout", "this.className='widget nodrag'");
        }

        if (WidgetInstance.Widget.IsLocked)
        {
            EditWidget.Visible = false;
            LockedWidget.Visible = true;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var widget = LoadControl(this.WidgetInstance.Widget.Url);
        widget.ID = "Widget" + this.WidgetInstance.Id.ToString();

        //WidgetBodyUpdatePanel.ContentTemplateContainer.Controls.Add(widget);
        WidgetBodyPanel.Controls.Add(widget);
        this._WidgetRef = widget as IWidget;
        this._WidgetRef.Init(this);
    }

    private void SetExpandCollapseButtons()
    {
        if (!this.WidgetInstance.Expanded)
        {
            ExpandWidget.Style.Add("display", "block");
            CollapseWidget.Style.Add("display", "none"); ;
            WidgetResizeFrame.Style.Add("display", "none");// .Visible = false;            
        }
        else
        {
            ExpandWidget.Style.Add("display", "none");
            CollapseWidget.Style.Add("display", "block");
            WidgetResizeFrame.Style.Add("display", "block");
        }
    }

    private void SetMaximizeRestoreButtons()
    {
        if (!this.WidgetInstance.Maximized)
        {
            MaximizeWidget.Style.Add("display", "block");
            RestoreWidget.Style.Add("display", "none");
        }
        else
        {
            MaximizeWidget.Style.Add("display", "none");
            RestoreWidget.Style.Add("display", "block");
        }
    }

    protected void EditWidget_Click(object sender, EventArgs e)
    {
        if (this.SettingsOpen)
        {
            (this as IWidgetHost).HideSettings();
        }
        else
        {
            (this as IWidgetHost).ShowSettings();
        }

        WidgetBodyUpdatePanel.Update();
    }

    protected void CollapseWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Collaspe();
    }

    protected void ExpandWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Expand();
    }

    protected void MaximizeWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Maximize();
    }

    protected void RestoreWidget_Click(object sender, EventArgs e)
    {
        (this as IWidgetHost).Restore();
    }

    protected void CloseWidget_Click(object sender, EventArgs e)
    {
        this._WidgetRef.Closed();
        (this as IWidgetHost).Close();
    }

    protected void SaveWidgetTitle_Click(object sender, EventArgs e)
    {
        WidgetTitleTextBox.Visible = SaveWidgetTitle.Visible = false;
        WidgetTitle.Visible = true;
        WidgetTitle.Text = WidgetTitleTextBox.Text;

        /*
        DetachAssociation(new Action(delegate()
        {
            DatabaseHelper.Update<WidgetInstance>(this.WidgetInstance, delegate(WidgetInstance wi)
            {
                wi.Title = WidgetTitleTextBox.Text;
            });
        }));
        */

    }

    protected void WidgetTitle_Click(object sender, EventArgs e)
    {
        WidgetTitleTextBox.Text = this.WidgetInstance.Title;
        WidgetTitleTextBox.Visible = true;
        SaveWidgetTitle.Visible = true;
        WidgetTitle.Visible = false;
    }

    protected void CancelEditWidget_Click(object sender, EventArgs e)
    {

    }

    int IWidgetHost.ID
    {
        get
        {
            return this.WidgetInstance.Id;
        }
    }

    void IWidgetHost.Maximize()
    {
        (this as IWidgetHost).Expand();

        //DetachAssociation(new Action(delegate()
        //{
        //    DatabaseHelper.UpdateObject<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance,
        //        this.WidgetInstance, (wi) =>
        //        {
        //            wi.Maximized = true;
        //        });
        //}));


        this.SetMaximizeRestoreButtons();
        this._WidgetRef.Maximized();

        //WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();

        ScriptManager.RegisterClientScriptBlock(this.WidgetResizeFrame, this.WidgetResizeFrame.GetType(), this.ID + "_maximizeWidget_" + DateTime.Now.Ticks.ToString(),
                                "DropthingsUI.Actions.maximizeWidget('" + this.WidgetInstance.Id + "', 'true');", true);
    }

    void IWidgetHost.Restore()
    {
        //DetachAssociation(new Action(delegate()
        //{
        //    DatabaseHelper.UpdateObject<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance,
        //        this.WidgetInstance, (wi) =>
        //        {
        //            wi.Maximized = false;
        //        });
        //}));

        this.SetMaximizeRestoreButtons();
        this._WidgetRef.Restored();

        //WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.Expand()
    {
        //DetachAssociation(new Action(delegate()
        //{
        //    DatabaseHelper.UpdateObject<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance,
        //        this.WidgetInstance, (wi) =>
        //        {
        //            wi.Expanded = true;
        //        });
        //}));


        this.SetExpandCollapseButtons();
        this._WidgetRef.Expanded();

        WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();

        ScriptManager.RegisterClientScriptBlock(this.ExpandWidget, this.ExpandWidget.GetType(), this.ID + "_expandWidget_" + DateTime.Now.Ticks.ToString(),
                                "DropthingsUI.setActionOnWidget('" + this.Widget.ClientID + "');", true);
    }

    void IWidgetHost.Collaspe()
    {
        //DetachAssociation(new Action(delegate()
        //{
        //    DatabaseHelper.UpdateObject<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance,
        //        this.WidgetInstance, (wi) =>
        //        {
        //            wi.Expanded = false;
        //        });
        //}));

        this.SetExpandCollapseButtons();
        this._WidgetRef.Collasped();

        WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();

        ScriptManager.RegisterClientScriptBlock(this.CollapseWidget, this.CollapseWidget.GetType(), this.ID + "_collaspeWidget_" + DateTime.Now.Ticks.ToString(),
                                "DropthingsUI.setActionOnWidget('" + this.Widget.ClientID + "');", true);
    }

    void IWidgetHost.Close()
    {
        ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeleteWidgetInstanceWorkflow,
                        DeleteWidgetInstanceWorkflowRequest,
                        DeleteWidgetInstanceWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = this.WidgetInstance.Id, UserName = Profile.UserName }
                        );

        //new DashboardFacade(Profile.UserName).DeleteWidgetInstance(this.WidgetInstance.Id);
        Deleted(this.WidgetInstance, this);        
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
        writer.AddAttribute(ATTR_INSTANCE_ID, this.WidgetInstance.Id.ToString());
        writer.AddAttribute(ATTR_RESIZED, this.WidgetInstance.Resized.ToString());
        writer.AddAttribute(ATTR_EXPANDED, this.WidgetInstance.Expanded.ToString());
        writer.AddAttribute(ATTR_MAXIMIZED, this.WidgetInstance.Maximized.ToString());
        writer.AddAttribute(ATTR_WIDTH, this.WidgetInstance.Width.ToString());
        writer.AddAttribute(ATTR_HEIGHT, this.WidgetInstance.Height.ToString());

        base.RenderControl(writer);
    }

    void IWidgetHost.SaveState(string state)
    {
        ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        SaveWidgetInstanceStateWorkflow,
                        SaveWidgetInstanceStateRequest,
                        SaveWidgetInstanceStateResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new SaveWidgetInstanceStateRequest { WidgetInstanceId = this._WidgetInstance.Id, State = state, UserName = Profile.UserName }
                        );

        // Invalidate cache because widget's state is stored in cache
        Cache.Remove(Profile.UserName);
    }

    /// <summary>
    /// Detach associated objects from WidgetInstance so that
    /// they do not get inserted again
    /// </summary>
    /// <param name="a"></param>
    private void DetachAssociation(Action a)
    {
        //var pageRef = this.WidgetInstance.Page;
        //var widgetRef = this.WidgetInstance.Widget;

        //this.WidgetInstance.Detach();

        a.Invoke();

        //this.WidgetInstance.Detach();

        //this.WidgetInstance.Page = pageRef;
        //this.WidgetInstance.Widget = widgetRef;
    }

    string IWidgetHost.GetState()
    {
        return this.WidgetInstance.State;
    }

    bool IWidgetHost.IsFirstLoad
    {
        get
        {
            return this.IsFirstLoad;
        }
        set
        {
            this.IsFirstLoad = value;
        }
    }

    void IWidgetHost.ShowSettings()
    {
        this.SettingsOpen = true;
        this._WidgetRef.ShowSettings();
        (this as IWidgetHost).Expand();
        EditWidget.Visible = false;
        CancelEditWidget.Visible = true;
        this.WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.HideSettings()
    {
        this.SettingsOpen = false;
        this._WidgetRef.HideSettings();
        EditWidget.Visible = true;
        CancelEditWidget.Visible = false;
        this.WidgetHeaderUpdatePanel.Update();
    }

}
