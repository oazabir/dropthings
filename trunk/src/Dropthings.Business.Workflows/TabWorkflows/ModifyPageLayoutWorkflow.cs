namespace Dropthings.Business
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

    using Dropthings.Business.Workflows.TabWorkflows;

    public sealed partial class ModifyPageLayoutWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(ModifyTabLayoutWorkflowRequest), typeof(ModifyPageLayoutWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(ModifyTabLayoutWorkflowResponse), typeof(ModifyPageLayoutWorkflow));

        public static DependencyProperty ZoneToRemoveProperty = DependencyProperty.Register("ZoneToRemove", typeof(Dropthings.DataAccess.WidgetZone), typeof(Dropthings.Business.ModifyPageLayoutWorkflow));

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
                return ((Dropthings.DataAccess.WidgetZone)(base.GetValue(Dropthings.Business.ModifyPageLayoutWorkflow.ZoneToRemoveProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.ModifyPageLayoutWorkflow.ZoneToRemoveProperty, value);
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
    }
}