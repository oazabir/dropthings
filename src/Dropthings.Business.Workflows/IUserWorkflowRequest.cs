namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IUserWorkflowRequest : IWorkflowRequest
    {
        #region Properties

        string UserName
        {
            get; set;
        }

        #endregion Properties
    }
}