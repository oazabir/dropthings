namespace Dropthings.Business.Workflows.UserAccountWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.DataAccess;

    public class UserRegistrationWorkflowResponse : UserWorkflowResponseBase, IUserVisitWorkflowResponse
    {
        #region Properties

        public Page CurrentPage
        {
            get; set;
        }

        public string UnlockKey
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

        public List<WidgetInstance> WidgetInstances
        {
            get; set;
        }

        #endregion Properties
    }
}