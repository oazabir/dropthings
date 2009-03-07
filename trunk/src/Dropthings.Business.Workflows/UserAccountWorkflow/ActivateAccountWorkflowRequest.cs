namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ActivateAccountWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public string ActivationKey
        {
            get; set;
        }

        #endregion Properties
    }
}