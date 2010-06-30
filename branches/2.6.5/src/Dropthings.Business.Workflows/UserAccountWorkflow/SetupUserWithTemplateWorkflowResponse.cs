namespace Dropthings.Business.Workflows.UserAccountWorkflow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.DataAccess;

    public class SetupUserWithTemplateWorkflowResponse : UserWorkflowResponseBase, IUserVisitWorkflowResponse
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

        public List<WidgetInstance> WidgetInstances
        {
            get; set;
        }

        #endregion Properties
    }
}