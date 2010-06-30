namespace Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GetWidgetWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int WidgetId
        {
            get; set;
        }

        #endregion Properties
    }
}