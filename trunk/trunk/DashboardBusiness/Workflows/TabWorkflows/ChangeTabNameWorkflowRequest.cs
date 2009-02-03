namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ChangeTabNameWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public string PageName
        {
            get; set;
        }

        #endregion Properties
    }
}