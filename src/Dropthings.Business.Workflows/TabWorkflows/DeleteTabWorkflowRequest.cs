namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DeleteTabWorkflowRequest : IUserWorkflowRequest
    {
        #region Properties

        public int PageID
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