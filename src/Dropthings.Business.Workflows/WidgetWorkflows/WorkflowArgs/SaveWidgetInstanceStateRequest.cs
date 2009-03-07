namespace Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SaveWidgetInstanceStateRequest : UserWorkflowRequestBase
    {
        #region Properties

        public string State
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