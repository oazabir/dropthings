namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class AddNewTabWorkflowResponse : IUserWorkflowResponse
    {
        #region Properties

        public Page NewPage
        {
            get; set;
        }

        public Guid UserGuid
        {
            get; set;
        }

        #endregion Properties
    }
}