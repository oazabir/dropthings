namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class UserWorkflowResponseBase : IUserWorkflowResponse
    {
        #region Properties

        public Guid UserGuid
        {
            set; get;
        }

        #endregion Properties
    }
}