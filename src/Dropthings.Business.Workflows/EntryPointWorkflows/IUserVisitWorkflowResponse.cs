namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;
    using System.Collections.Generic;

    using Dropthings.DataAccess;

    public interface IUserVisitWorkflowResponse : IUserWorkflowResponse
    {
        #region Properties

        Page CurrentPage
        {
            get; set;
        }

        List<Page> UserPages
        {
            get; set;
        }

        UserSetting UserSetting
        {
            get; set;
        }

        #endregion Properties
    }
}