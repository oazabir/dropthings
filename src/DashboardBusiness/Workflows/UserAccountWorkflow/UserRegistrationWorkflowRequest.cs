namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;

    public class UserRegistrationWorkflowRequest : UserWorkflowRequestBase, IUserVisitWorkflowRequest
    {
        #region Properties

        public string Email
        {
            get; set;
        }

        public bool IsActivationRequired
        {
            get; set;
        }

        public string PageName
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public string RequestedUsername
        {
            get; set;
        }

        public string RoleName
        {
            get; set;
        }

        #endregion Properties
    }
}