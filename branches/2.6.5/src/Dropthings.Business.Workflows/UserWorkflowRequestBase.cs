namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class UserWorkflowRequestBase : IUserWorkflowRequest
    {
        #region Properties

        public bool IsAnonymous
        {
            get; set;
        }

        public string UserName
        {
            get; set;
        }

        #endregion Properties
    }
}