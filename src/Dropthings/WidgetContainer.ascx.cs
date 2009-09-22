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
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.WidgetWorkflows;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;
using Dropthings.Web.Framework;

using Dropthings.Business.Facade;
using Dropthings.Model;
using Dropthings.Business.Facade.Context;

public partial class WidgetContainer : System.Web.UI.UserControl, IWidgetHost
{
    public const string ATTR_INSTANCEID = "_InstanceId";
    public const string ATTR_INSTANCE_ID = "_InstanceId";
    public const string ATTR_RESIZED = "_Resized";
    public const string ATTR_EXPANDED = "_Expanded";
    public const string ATTR_MAXIMIZED = "_Maximized";
    public const string ATTR_WIDTH = "_Width";
    public const string ATTR_HEIGHT = "_Height";
    private const string ATTR_ZONE_ID = "_zoneid";

    public event Action<WidgetInstance,IWidgetHost> Deleted;

    public override string UniqueID
    {
        get
        {
            return "Widget" + this.WidgetInstance.Id.ToString();
        }
    }

    public override string ClientID
    {
        get
        {
            return "Widget" + this.WidgetInstance.Id.ToString();
        }
    }    

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

    public bool IsFirstLoad { get; set; }

    public bool IsLocked { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (this.WidgetInstance.Resized)
        {
            this.WidgetResizeFrame.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
            this.WidgetResizeFrame.Style.Add(HtmlTextWriterStyle.Height, this.WidgetInstance.Height + "px");
        }

        if (this.WidgetInstance.Expanded && (!IsLocked || !WidgetInstance.Widget.IsLocked))
        {
            ScriptManager.RegisterStartupScript(this.WidgetResizeFrame, this.WidgetResizeFrame.GetType(), this.ID + "_initResizer",
                "DropthingsUI.initResizer('" + this.WidgetResizeFrame.ClientID + "');", true);
        }
        else
        {
            this.WidgetResizeFrame.Style.Add("display", "none");
            //this.WidgetResizeFrame.Visible = false;
        }

        //writer.AddAttribute(ATTR_RESIZED, this.WidgetInstance.Resized.ToString());
        //writer.AddAttribute(ATTR_EXPANDED, this.WidgetInstance.Expanded.ToString());
        //writer.AddAttribute(ATTR_MAXIMIZED, this.WidgetInstance.Maximized.ToString());
        //writer.AddAttribute(ATTR_WIDTH, this.WidgetInstance.Width.ToString());
        //writer.AddAttribute(ATTR_HEIGHT, this.WidgetInstance.Height.ToString());
        ScriptManager.RegisterStartupScript(this.WidgetHeaderUpdatePanel, typeof(UpdatePanel), "SetWidgetDef" + this.WidgetInstance.Id, 
            "DropthingsUI.setWidgetDef(/*id*/ '{0}', /*expanded*/ {1}, /*maximized*/ {2}, /*resized*/ {3}, /*width*/ {4}, /*height*/ {5}, /*zoneId*/ {6});"
                .FormatWith(
                    this.WidgetInstance.Id,
                    this.WidgetInstance.Expanded.ToString().ToLower(),
                    this.WidgetInstance.Maximized.ToString().ToLower(),
                    this.WidgetInstance.Resized.ToString().ToLower(),
                    this.WidgetInstance.Width,
                    this.WidgetInstance.Height,
                    this.WidgetInstance.WidgetZoneId) + 
            "DropthingsUI.setActionOnWidget('" + this.Widget.ClientID + "');",
            true);        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WidgetTitle.Text = this.WidgetInstance.Title;
        this.SetExpandCollapseButtons();
        this.SetMaximizeRestoreButtons();
        WidgetTitleTextBox.Style.Add("display", "none");
        SaveWidgetTitle.Style.Add("display", "none");

        if (this.IsLocked || WidgetInstance.Widget.IsLocked || this.WidgetInstance.Maximized)
        {
            Widget.CssClass = "widget nodrag";
            Widget.Attributes.Add("onmouseover", "this.className='widget nodrag widget_hover'");
            Widget.Attributes.Add("onmouseout", "this.className='widget nodrag'");
        }

        if (IsLocked || WidgetInstance.Widget.IsLocked)
        {
            WidgetHeader.Enabled = false;
            EditWidget.Visible = MaximizeWidget.Visible = RestoreWidget.Visible = CollapseWidget.Visible = ExpandWidget.Visible = CloseWidget.Visible = false;
            LockedWidget.Visible = true;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var widget = LoadControl(this.WidgetInstance.Widget.Url);
        widget.ID = "Widget" + this.WidgetInstance.Id.ToString();

        WidgetBodyPanel.Controls.Add(widget);
        _WidgetRef = widget as IWidget;
        if (_WidgetRef != null) _WidgetRef.Init(this);

        // Since viewstate is disabled for all control, we need to do things manually            
        if (this.SettingsOpen)
        {
            // Since viewstate is disabled for all control, we need to do things manually
            EditWidget.Visible = false;
            CancelEditWidget.Visible = true;
        }
        else
        {
            EditWidget.Visible = true;
            CancelEditWidget.Visible = false;
        }
    }

    private void SetExpandCollapseButtons()
    {
        if (!this.WidgetInstance.Expanded)
        {
            ExpandWidget.Style.Add("display", "block");
            CollapseWidget.Style.Add("display", "none"); ;
            WidgetResizeFrame.Style.Add("display", "none");
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
    }

    protected void WidgetTitle_Click(object sender, EventArgs e)
    {
        WidgetTitleTextBox.Text = this.WidgetInstance.Title;
        WidgetTitleTextBox.Visible = true;
        SaveWidgetTitle.Visible = true;
        WidgetTitle.Visible = false;
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

        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            this.WidgetInstance = facade.MaximizeWidget(this.WidgetInstance.Id, true);
        }

        // -- Workflow way. Obselete.
        //this.ReloadWidgetInstanceAfter(() => 
        //    WorkflowHelper.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
        //        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = this.WidgetInstance.Id, IsMaximize = true }));

        this.SetMaximizeRestoreButtons();
        this._WidgetRef.Maximized();

        //WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();

        //ScriptManager.RegisterClientScriptBlock(this.WidgetResizeFrame, this.WidgetResizeFrame.GetType(), this.ID + "_maximizeWidget_" + DateTime.Now.Ticks.ToString(),
        //                        "DropthingsUI.Actions.maximizeWidget('" + this.WidgetInstance.Id + "', 'true');", true);
    }

    void IWidgetHost.Restore()
    {
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            this.WidgetInstance = facade.MaximizeWidget(this.WidgetInstance.Id, false);
        }

        // -- Workflow way. Obselete.
        //this.ReloadWidgetInstanceAfter(() => 
        //    WorkflowHelper.Run<MaximizeWidgetInstanceWorkflow, MaximizeWidgetInstanceRequest, MaximizeWidgetInstanceResponse>(
        //        new MaximizeWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = this.WidgetInstance.Id, IsMaximize = false }));

        this.SetMaximizeRestoreButtons();
        this._WidgetRef.Restored();

        WidgetHeaderUpdatePanel.Update();
    }

    void IWidgetHost.Expand()
    {
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            this.WidgetInstance = facade.ExpandWidget(this.WidgetInstance.Id, true);
        }

        // -- Workflow way. Obselete.
        //this.ReloadWidgetInstanceAfter(() => 
        //    WorkflowHelper.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
        //        new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = this.WidgetInstance.Id, IsExpand = true }));

        this.SetExpandCollapseButtons();
        this._WidgetRef.Expanded();

        WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();        
    }

    void IWidgetHost.Collaspe()
    {
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            this.WidgetInstance = facade.ExpandWidget(this.WidgetInstance.Id, false);
        }

        // -- Workflow way. Obselete.
        //this.ReloadWidgetInstanceAfter(() => 
        //    WorkflowHelper.Run<ExpandWidgetInstanceWorkflow, ExpandWidgetInstanceRequest, ExpandWidgetInstanceResponse>(
        //        new ExpandWidgetInstanceRequest { UserName = Profile.UserName, WidgetInstanceId = this.WidgetInstance.Id, IsExpand = false }));

        this.SetExpandCollapseButtons();
        this._WidgetRef.Collasped();

        WidgetBodyUpdatePanel.Update();
        WidgetHeaderUpdatePanel.Update();

    }

    void IWidgetHost.Close()
    {
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            facade.DeleteWidgetInstance(this.WidgetInstance.Id);
        }

        // -- Workflow way. Obselete.
        //WorkflowHelper.Run<DeleteWidgetInstanceWorkflow, DeleteWidgetInstanceWorkflowRequest, DeleteWidgetInstanceWorkflowResponse>(
        //                    new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = this.WidgetInstance.Id, UserName = Profile.UserName }
        //                );

        Deleted(this.WidgetInstance, this);
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
        writer.AddAttribute(ATTR_INSTANCE_ID, this.WidgetInstance.Id.ToString());
        writer.AddAttribute(ATTR_ZONE_ID, this.WidgetInstance.WidgetZoneId.ToString());
        //writer.AddAttribute(ATTR_RESIZED, this.WidgetInstance.Resized.ToString());
        //writer.AddAttribute(ATTR_EXPANDED, this.WidgetInstance.Expanded.ToString());
        //writer.AddAttribute(ATTR_MAXIMIZED, this.WidgetInstance.Maximized.ToString());
        //writer.AddAttribute(ATTR_WIDTH, this.WidgetInstance.Width.ToString());
        //writer.AddAttribute(ATTR_HEIGHT, this.WidgetInstance.Height.ToString());

        base.RenderControl(writer);
    }

    void IWidgetHost.SaveState(string state)
    {
        using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
        {
            this.WidgetInstance = facade.SaveWidgetInstanceState(this._WidgetInstance.Id, state);
        }

        // -- Workflow way. Obselete.
        //this.ReloadWidgetInstanceAfter(() =>
        //    WorkflowHelper.Run<SaveWidgetInstanceStateWorkflow, SaveWidgetInstanceStateRequest, SaveWidgetInstanceStateResponse>(
        //        new SaveWidgetInstanceStateRequest { WidgetInstanceId = this._WidgetInstance.Id, State = state, UserName = Profile.UserName }));
                        
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

    void IWidgetHost.ShowSettings()
    {
        this.SettingsOpen = true;
        this._WidgetRef.ShowSettings();
        (this as IWidgetHost).Expand();
        EditWidget.Visible = false;
        CancelEditWidget.Visible = true;
        this.WidgetHeaderUpdatePanel.Update();
        this.WidgetBodyUpdatePanel.Update();
    }

    void IWidgetHost.HideSettings()
    {
        this.SettingsOpen = false;
        this._WidgetRef.HideSettings();
        EditWidget.Visible = true;
        CancelEditWidget.Visible = false;
        this.WidgetHeaderUpdatePanel.Update();
        this.WidgetBodyUpdatePanel.Update();
    }


    #region IWidgetHost Members


    void IWidgetHost.Refresh(IWidget widget)
    {
        this.WidgetHeaderUpdatePanel.Update();
        this.WidgetBodyUpdatePanel.Update();
    }

    EventBrokerService IWidgetHost.EventBroker
    {
        get;
        set;
    }

    #endregion

    protected void Refresh_Clicked(object sender, EventArgs e)
    {
        (this as IWidgetHost).Refresh(_WidgetRef);
    }

    protected void ReloadWidgetInstanceAfter(Func<WidgetInstanceResponseBase> doSomething)
    {
        this.WidgetInstance = doSomething().WidgetInstanceAffected;        
    }
}
