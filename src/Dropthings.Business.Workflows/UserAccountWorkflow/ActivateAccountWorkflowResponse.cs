namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class ActivateAccountWorkflowResponse : UserWorkflowResponseBase
    {
        #region Properties

        public Token Token
        {
            get; set;
        }

        #endregion Properties
    }
}