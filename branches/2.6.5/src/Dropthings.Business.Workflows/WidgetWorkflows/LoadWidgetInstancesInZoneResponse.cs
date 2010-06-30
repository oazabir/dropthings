namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class LoadWidgetInstancesInZoneResponse : UserWorkflowResponseBase
    {
        #region Properties

        public List<WidgetInstance> WidgetInstances
        {
            get; set;
        }

        #endregion Properties
    }
}