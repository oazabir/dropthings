namespace Dropthings.Business.Workflows.TabWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class GetColumnsInPageWorkflowResponse : IUserWorkflowResponse
    {
        #region Properties

        public List<Column> Columns
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