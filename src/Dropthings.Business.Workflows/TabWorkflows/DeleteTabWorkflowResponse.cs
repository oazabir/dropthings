namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class DeleteTabWorkflowResponse : UserWorkflowResponseBase
    {
        #region Properties

        public Page NewCurrentPage
        {
            get; set;
        }

        #endregion Properties
    }
}