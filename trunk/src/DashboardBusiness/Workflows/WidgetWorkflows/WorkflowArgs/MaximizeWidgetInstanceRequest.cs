namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MaximizeWidgetInstanceRequest : UserWorkflowRequestBase
    {
        #region Properties

        public bool IsMaximize
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