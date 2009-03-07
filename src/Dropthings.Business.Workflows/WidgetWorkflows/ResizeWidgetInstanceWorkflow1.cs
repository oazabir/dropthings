using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    public sealed partial class ResizeWidgetInstanceWorkflow : SequentialWorkflowActivity
    {
        public ResizeWidgetInstanceWorkflow()
        {
            InitializeComponent();
        }

        public ResizeWidgetInstanceRequest Request
        {
            get { return (ResizeWidgetInstanceRequest)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Request.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RequestProperty =
            DependencyProperty.Register("Request", typeof(ResizeWidgetInstanceRequest), typeof(ResizeWidgetInstanceWorkflow));


        public ResizeWidgetInstanceResponse Response
        {
            get { return (ResizeWidgetInstanceResponse)GetValue(ResponseProperty); }
            set { SetValue(ResponseProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Response.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResponseProperty =
            DependencyProperty.Register("Response", typeof(ResizeWidgetInstanceResponse), typeof(ResizeWidgetInstanceWorkflow));
    }

}
