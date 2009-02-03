namespace Dropthings.Business.Workflows.WidgetWorkflows
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