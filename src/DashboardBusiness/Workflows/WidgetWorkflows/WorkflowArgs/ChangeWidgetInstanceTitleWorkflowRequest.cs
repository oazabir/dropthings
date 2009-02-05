namespace Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ChangeWidgetInstanceTitleWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public string NewTitle
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