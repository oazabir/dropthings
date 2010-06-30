namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class UserVisitWorkflowResponse : UserWorkflowResponseBase, IUserVisitWorkflowResponse
    {
        #region Properties

        public Page CurrentPage
        {
            get; set;
        }

        public List<Page> UserPages
        {
            get; set;
        }

        public UserSetting UserSetting
        {
            get; set;
        }

        #endregion Properties
    }
}