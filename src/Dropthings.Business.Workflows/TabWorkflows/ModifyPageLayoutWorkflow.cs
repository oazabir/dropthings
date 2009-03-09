namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;
    using Dropthings.Business.Activities;
    using Dropthings.DataAccess;

    public sealed partial class ModifyPageLayoutWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty =
            DependencyProperty.Register("Request", typeof(ModifyTabLayoutWorkflowRequest), typeof(ModifyPageLayoutWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty =
            DependencyProperty.Register("Response", typeof(ModifyTabLayoutWorkflowResponse), typeof(ModifyPageLayoutWorkflow));

        public static DependencyProperty ZoneToRemoveProperty = DependencyProperty.Register("ZoneToRemove", typeof(Dropthings.DataAccess.WidgetZone), typeof(ModifyPageLayoutWorkflow));

        public int ColumnCounter = 0;
        public Dropthings.DataAccess.Page CurrentPage = new Dropthings.DataAccess.Page();
        public System.Collections.Generic.List<Dropthings.DataAccess.Column> ExistingColumns = new System.Collections.Generic.List<Dropthings.DataAccess.Column>();
        public int[] NewColumnDefs = default(System.Int32[]);
        public int NewColumnNo = 0;

        #endregion Fields

        #region Constructors

        public ModifyPageLayoutWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ModifyTabLayoutWorkflowRequest Request
        {
            get { return (ModifyTabLayoutWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public ModifyTabLayoutWorkflowResponse Response
        {
            get { return (ModifyTabLayoutWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        [System.ComponentModel.DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Misc")]
        public Dropthings.DataAccess.WidgetZone ZoneToRemove
        {
            get
            {
                return ((Dropthings.DataAccess.WidgetZone)(base.GetValue(ModifyPageLayoutWorkflow.ZoneToRemoveProperty)));
            }
            set
            {
                base.SetValue(ModifyPageLayoutWorkflow.ZoneToRemoveProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        private void LoadPage_ExecuteCode(object sender, EventArgs e)
        {
        }

        private void SetCounters_ExecuteCode(object sender, EventArgs e)
        {
            this.ColumnCounter = this.ExistingColumns.Count - 1;
            this.NewColumnNo = this.NewColumnDefs.Length - 1;
        }

        #endregion Methods

        public Dropthings.DataAccess.WidgetZone OldWidgetZone = new Dropthings.DataAccess.WidgetZone();

        private void DecreaseColumnCounter_ExecuteCode(object sender, EventArgs e)
        {
            this.ColumnCounter--;
        }

        public Dropthings.DataAccess.WidgetZone NewWidgetZone = new Dropthings.DataAccess.WidgetZone();
        public System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> WidgetInstancesToMove = new System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance>();

        private void ForEachWidgetInstance_Iterating(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = sender as ForEachActivity.ForEach;
            ChangeWidgetInstancePositionActivity changePositionActivity = forEachActivity.DynamicActivity as ChangeWidgetInstancePositionActivity;
            WidgetInstance widgetInstance = forEachActivity.Enumerator.Current as WidgetInstance;
            changePositionActivity.WidgetInstanceId = widgetInstance.Id;

        }

        private void IncreaseColumnCounter_ExecuteCode(object sender, EventArgs e)
        {
            this.ColumnCounter++;
        }

        public int NewColumnWidth = 0;
        private void SetWidthForColumn_ExecuteCode(object sender, EventArgs e)
        {
            this.NewColumnWidth = this.NewColumnDefs[this.ColumnCounter];
        }
    }
}