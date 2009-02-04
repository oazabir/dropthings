namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UserVisitWorkflowRequest : UserWorkflowRequestBase, IUserVisitWorkflowRequest
    {
        #region Properties

        public string PageName
        {
            get; set;
        }

        #endregion Properties
    }
}