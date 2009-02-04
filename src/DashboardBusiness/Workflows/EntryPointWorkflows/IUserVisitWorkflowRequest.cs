namespace Dropthings.Business.Workflows.EntryPointWorkflows
{
    using System;

    public interface IUserVisitWorkflowRequest : IUserWorkflowRequest
    {
        #region Properties

        bool IsAnonymous
        {
            get; set;
        }

        string PageName
        {
            get; set;
        }

        #endregion Properties
    }
}