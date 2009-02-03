namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MoveWidgetInstanceWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int NewZoneId
        {
            get; set;
        }

        public int RowNo
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