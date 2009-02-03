namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GetWidgetInstanceStateRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int WidgetInstanceId
        {
            get; set;
        }

        #endregion Properties
    }
}