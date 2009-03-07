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

    using Dropthings.DataAccess;

    public partial class LoadWidgetInstancesInZoneWorkflow : SequentialWorkflowActivity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty = 
            DependencyProperty.Register("Request", typeof(LoadWidgetInstancesInZoneRequest), typeof(LoadWidgetInstancesInZoneWorkflow));

        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty = 
            DependencyProperty.Register("Response", typeof(LoadWidgetInstancesInZoneResponse), typeof(LoadWidgetInstancesInZoneWorkflow));

        #endregion Fields

        #region Properties

        public LoadWidgetInstancesInZoneRequest Request
        {
            get { return (LoadWidgetInstancesInZoneRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        public LoadWidgetInstancesInZoneResponse Response
        {
            get { return (LoadWidgetInstancesInZoneResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }

        #endregion Properties
    }
}