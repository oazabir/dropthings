namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IUserWorkflowResponse : IWorkflowResponse
    {
        #region Properties

        Guid UserGuid
        {
            get; set;
        }

        #endregion Properties
    }
}