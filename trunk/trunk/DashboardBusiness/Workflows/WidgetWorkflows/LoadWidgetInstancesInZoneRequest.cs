namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LoadWidgetInstancesInZoneRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int WidgetZoneId
        {
            get; set;
        }

        #endregion Properties
    }
}