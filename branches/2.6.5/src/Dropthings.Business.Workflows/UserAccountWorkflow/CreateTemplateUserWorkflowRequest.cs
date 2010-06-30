namespace Dropthings.Business.Workflows.UserAccountWorkflow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.UserAccountWorkflows;

    public class CreateTemplateUserWorkflowRequest : UserRegistrationWorkflowRequest
    {
        #region Properties

        public string TemplateRoleName
        {
            get; set;
        }

        #endregion Properties
    }
}