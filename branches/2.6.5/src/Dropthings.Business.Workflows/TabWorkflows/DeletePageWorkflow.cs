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
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.DataAccess;

    public sealed partial class DeletePageWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(DeleteTabWorkflowRequest), typeof(DeletePageWorkflow));
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(DeleteTabWorkflowResponse), typeof(DeletePageWorkflow));

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(System.Collections.Generic.List<Dropthings.DataAccess.Column>), typeof(DeletePageWorkflow));
        public static DependencyProperty CurrentPageIdProperty = DependencyProperty.Register("CurrentPageId", typeof(System.Int32), typeof(DeletePageWorkflow));
        public static DependencyProperty WidgetInstancesProperty = DependencyProperty.Register("WidgetInstances", typeof(System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance>), typeof(DeletePageWorkflow));

        #endregion Fields

        #region Constructors

        public DeletePageWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public System.Collections.Generic.List<Dropthings.DataAccess.Column> Columns
        {
            get
            {
                return ((System.Collections.Generic.List<Dropthings.DataAccess.Column>)(base.GetValue(DeletePageWorkflow.ColumnsProperty)));
            }
            set
            {
                base.SetValue(DeletePageWorkflow.ColumnsProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 CurrentPageId
        {
            get
            {
                return ((int)(base.GetValue(DeletePageWorkflow.CurrentPageIdProperty)));
            }
            set
            {
                base.SetValue(DeletePageWorkflow.CurrentPageIdProperty, value);
            }
        }

        public DeleteTabWorkflowRequest Request
        {
            get { return (DeleteTabWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public DeleteTabWorkflowResponse Response
        {
            get { return (DeleteTabWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> WidgetInstances
        {
            get
            {
                return ((System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance>)(base.GetValue(DeletePageWorkflow.WidgetInstancesProperty)));
            }
            set
            {
                base.SetValue(DeletePageWorkflow.WidgetInstancesProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        private void OnForEachColumn(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var columnToDelete = forEachActivity.Enumerator.Current as Column;
            var sActivity = forEachActivity.DynamicActivity as SequenceActivity;

            (sActivity.Activities.OfType<GetWidgetZoneActivity>().First() as GetWidgetZoneActivity).ZoneId = columnToDelete.WidgetZoneId;
            (sActivity.Activities.OfType<DeleteColumnActivity>().First() as DeleteColumnActivity).ColumnId = columnToDelete.ID;
        }

        private void OnForEachWidgetInstance(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var widgetInstanceToDelete = forEachActivity.Enumerator.Current as WidgetInstance;
            (forEachActivity.DynamicActivity as DeleteWidgetInstanceActivity).WidgetInstanceId = widgetInstanceToDelete.Id;
        }

        #endregion Methods
    }
}