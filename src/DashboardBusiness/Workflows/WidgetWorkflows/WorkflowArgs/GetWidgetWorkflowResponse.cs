namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class GetWidgetWorkflowResponse : UserWorkflowResponseBase
    {
        #region Properties

        public Widget Widget
        {
            get; set;
        }

        #endregion Properties
    }
}