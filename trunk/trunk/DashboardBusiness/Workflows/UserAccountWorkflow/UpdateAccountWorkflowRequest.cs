namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UpdateAccountWorkflowRequest : UserWorkflowRequestBase
    {
        #region Properties

        public string Email
        {
            get; set;
        }

        #endregion Properties
    }
}