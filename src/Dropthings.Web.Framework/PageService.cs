#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for WidgetService
/// </summary>
namespace Dropthings.Web.Framework
{
    using System;
    using System.Collections;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using System.Workflow.Runtime;

    using Dropthings.Business;
    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows;
    using Dropthings.DataAccess;
    using Dropthings.Web.Framework;

    using Dropthings.Business.Facade;
    using Dropthings.Business.Facade.Context;

    public class PageService : WebServiceBase
    {
        #region Constructors

        public PageService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeCurrentPage(int pageId)
        {
            //new Dropthings.Business.DashboardFacade(Profile.UserName).ChangeCurrentTab(pageId);

            WorkflowHelper.Run<ChangeTabWorkflow, ChangeTabWorkflowRequest, ChangeTabWorkflowResponse>(
                new ChangeTabWorkflowRequest { PageID = pageId, UserName = Profile.UserName }
            );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.ModifyPageLayout(newLayout);
            }

            // -- Workflow way. Obselete.
            //WorkflowHelper.Run<ModifyPageLayoutWorkflow, ModifyTabLayoutWorkflowRequest, ModifyTabLayoutWorkflowResponse>(
            //        new ModifyTabLayoutWorkflowRequest { UserName = Profile.UserName, LayoutType = newLayout }
            //        );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string DeletePage(int PageID)
        {
            var response = WorkflowHelper.Run<DeletePageWorkflow, DeleteTabWorkflowRequest, DeleteTabWorkflowResponse>(
                new DeleteTabWorkflowRequest { PageID = PageID, UserName = Profile.UserName }
            );

            Context.Cache.Remove(Profile.UserName);
            return response.NewCurrentPage.UserTabName;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string NewPage(string newLayout)
        {
            //var newPage = new DashboardFacade(Profile.UserName).AddNewPage(newLayout);
            //return newPage.TabName();

            var response = WorkflowHelper.Run<AddNewTabWorkflow, AddNewTabWorkflowRequest, AddNewTabWorkflowResponse>(
                new AddNewTabWorkflowRequest { LayoutType = newLayout, UserName = Profile.UserName }
            );

            return response.NewPage.UserTabName;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RenamePage(string newName)
        {
            WorkflowHelper.Run<ChangePageNameWorkflow, ChangeTabNameWorkflowRequest, ChangeTabNameWorkflowResponse>(
                new ChangeTabNameWorkflowRequest { IsAnonymous = Profile.IsAnonymous, PageName = newName, UserName = Profile.UserName }
            );
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void MoveTab(int pageId, int orderNo)
        {
            using (var facade = new Facade(new AppContext(string.Empty, Profile.UserName)))
            {
                facade.MovePage(pageId, orderNo);
            }
        }

        #endregion Methods
    }
}