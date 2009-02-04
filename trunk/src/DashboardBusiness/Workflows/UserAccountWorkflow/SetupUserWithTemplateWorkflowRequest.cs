namespace Dropthings.Business.Workflows.UserAccountWorkflow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;

    public class SetupUserWithTemplateWorkflowRequest : UserWorkflowRequestBase, IUserVisitWorkflowRequest
    {
        #region Properties

        public string CloneWithUserName
        {
            get; set;
        }

        public string PageName
        {
            get; set;
        }

        #endregion Properties
    }
}