namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class GetWidgetInstanceStateResponse : UserWorkflowResponseBase
    {
        #region Properties

        public string WidgetState
        {
            get; set;
        }

        #endregion Properties
    }
}