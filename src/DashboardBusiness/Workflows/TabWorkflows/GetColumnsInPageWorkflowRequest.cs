namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GetColumnsInPageWorkflowRequest : IUserWorkflowRequest
    {
        #region Properties

        public int PageId
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