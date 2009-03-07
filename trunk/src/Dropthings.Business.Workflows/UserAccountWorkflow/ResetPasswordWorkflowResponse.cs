namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class ResetPasswordWorkflowResponse : UserWorkflowResponseBase
    {
        #region Properties

        public string NewPassword
        {
            get; set;
        }

        #endregion Properties
    }
}