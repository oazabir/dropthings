namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ChangeTabWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int PageID
        {
            get; set;
        }

        #endregion Properties
    }
}