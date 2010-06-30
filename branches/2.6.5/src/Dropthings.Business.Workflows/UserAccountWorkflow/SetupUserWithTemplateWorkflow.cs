namespace Dropthings.Business.Workflows.UserAccountWorkflow
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
    using Dropthings.Business.Activities.PageActivities;
    using Dropthings.Business.Activities.WidgetActivities;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.DataAccess;

    public sealed partial class SetupUserWithTemplateWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(IUserVisitWorkflowRequest), typeof(SetupUserWithTemplateWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(IUserVisitWorkflowResponse), typeof(SetupUserWithTemplateWorkflow));

        private static DependencyProperty CloneWithUserNameProperty = DependencyProperty.Register("CloneWithUserName", typeof(String), typeof(SetupUserWithTemplateWorkflow));

        #endregion Fields

        #region Constructors

        public SetupUserWithTemplateWorkflow()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string CloneWithUserName
        {
            get { return (string)base.GetValue(CloneWithUserNameProperty); }
            set { base.SetValue(CloneWithUserNameProperty, value); }
        }

        public IUserVisitWorkflowRequest Request
        {
            get { return (IUserVisitWorkflowRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public IUserVisitWorkflowResponse Response
        {
            get { return (IUserVisitWorkflowResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties

        #region Methods

        private void OnForEachColumnOfPage(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var columnToClone = forEachActivity.Enumerator.Current as Column;
            var sActivity = forEachActivity.DynamicActivity as SequenceActivity;

            (sActivity.Activities.OfType<GetWidgetZoneActivity>().First() as GetWidgetZoneActivity).ZoneId = columnToClone.WidgetZoneId;
            (sActivity.Activities.OfType<GetWidgetInstancesInZoneActivity>().First() as GetWidgetInstancesInZoneActivity).WidgetZoneId = columnToClone.WidgetZoneId;
            (sActivity.Activities.OfType<CloneColumnActivity>().First() as CloneColumnActivity).ColumnToClone = columnToClone;
        }

        private void OnForEachPage(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var pageToClone = forEachActivity.Enumerator.Current as Page;
            var sActivity = forEachActivity.DynamicActivity as SequenceActivity;

            (sActivity.Activities.OfType<ClonePageActivity>().First() as ClonePageActivity).PageToClone = pageToClone;
            (sActivity.Activities.OfType<GetColumnsOfPageActivity>().First() as GetColumnsOfPageActivity).PageId = pageToClone.ID;
        }

        private void OnForEachWidgetInstance(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var widgetInstanceToClone = forEachActivity.Enumerator.Current as WidgetInstance;
            (forEachActivity.DynamicActivity as CloneWidgetInstanceActivity).WidgetInstance = widgetInstanceToClone;
        }

        #endregion Methods
    }
}