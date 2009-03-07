namespace Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ExpandWidgetInstanceRequest : UserWorkflowRequestBase
    {
        #region Properties

        public bool IsExpand
        {
            get; set;
        }

        public int WidgetInstanceId
        {
            get; set;
        }

        #endregion Properties
    }
}