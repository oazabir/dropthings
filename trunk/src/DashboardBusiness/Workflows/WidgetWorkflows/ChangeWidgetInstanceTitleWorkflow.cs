namespace Dropthings.Business.Workflows.WidgetWorkflows
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

    public sealed partial class ChangeWidgetInstanceTitleWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        public static DependencyProperty NewTitleProperty = DependencyProperty.Register("NewTitle", typeof(System.String), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));
        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(System.String), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));
        public static DependencyProperty WidgetInstanceIdProperty = DependencyProperty.Register("WidgetInstanceId", typeof(System.Int32), typeof(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow));

        #endregion Fields

        #region Constructors

        public ChangeWidgetInstanceTitleWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String NewTitle
        {
            get
            {
                return ((string)(base.GetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.NewTitleProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.NewTitleProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String UserName
        {
            get
            {
                return ((string)(base.GetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.UserNameProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.UserNameProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 WidgetInstanceId
        {
            get
            {
                return ((int)(base.GetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.WidgetInstanceIdProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Workflows.WidgetWorkflows.ChangeWidgetInstanceTitleWorkflow.WidgetInstanceIdProperty, value);
            }
        }

        #endregion Properties
    }
}