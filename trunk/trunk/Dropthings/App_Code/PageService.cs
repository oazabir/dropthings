// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Web.Script.Services;

using Dropthings.Business;
using Dropthings.DataAccess;
using Dropthings.Web.Framework;
using Dropthings.Business.Workflows.EntryPointWorkflows;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows;
using Dropthings.Business.Workflows.TabWorkflows;
using System.Workflow.Runtime;
using Dropthings.Business.Workflows.WidgetWorkflows;
/// <summary>
/// Summary description for WidgetService
/// </summary>

namespace Dropthings.Web.Framework
{
    public class PageService : WebServiceBase
    {

        public PageService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string NewPage(string newLayout)
        {
            //var newPage = new DashboardFacade(Profile.UserName).AddNewPage(newLayout);
            //return newPage.TabName();

            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        AddNewTabWorkflow,
                        AddNewTabWorkflowRequest,
                        AddNewTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new AddNewTabWorkflowRequest { LayoutType = newLayout, UserName = Profile.UserName }
                        );

            return response.NewPage.TabName();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangeCurrentPage(int pageId)
        {
            //new Dropthings.Business.DashboardFacade(Profile.UserName).ChangeCurrentTab(pageId);

            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ChangeTabWorkflow,
                        ChangeTabWorkflowRequest,
                        ChangeTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ChangeTabWorkflowRequest { PageID = pageId, UserName = Profile.UserName }
                        );
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public string DeletePage(int PageID)
        {
            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeletePageWorkflow,
                        DeleteTabWorkflowRequest,
                        DeleteTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new DeleteTabWorkflowRequest { PageID = PageID, UserName = Profile.UserName }
                        );

            Context.Cache.Remove(Profile.UserName);
            return response.NewCurrentPage.TabName();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void RenamePage(string newName)
        {
            new DashboardFacade(Profile.UserName).ChangePageName(newName);

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true)]
        public void ChangePageLayout(int newLayout)
        {
            //IUserVisitWorkflowResponse setup = new Dropthings.Business.DashboardFacade(Profile.UserName).LoadUserSetup(string.Empty);


            ////if new layout is 3 cols then do nothing
            ////if new layout is 2 cols we need to move the 3rd into the second
            //if (newLayout == 2 | newLayout == 3 | newLayout == 5)
            //{
            //    foreach (WidgetInstance instance in setup.WidgetInstances)
            //    {
            //        if (setup.CurrentPage.ID == instance.PageId)
            //        {
            //            if (instance.ColumnNo == 2)
            //            {
            //                //move widget to the middle panel
            //                new DashboardFacade(Profile.UserName).MoveWidgetInstance(instance.Id, 1, 0);
            //            }
            //        }
            //    }

            //}
            //else if (newLayout == 4)
            //{ //move the widgets from both middle panels to the left panel.

            //    foreach (WidgetInstance instance in setup.WidgetInstances)
            //    {
            //        if (setup.CurrentPage.ID == instance.PageId)
            //        {
            //            if (instance.ColumnNo == 1 | instance.ColumnNo == 2)
            //            {
            //                new DashboardFacade(Profile.UserName).MoveWidgetInstance(instance.Id, 0, 0);
            //            }
            //        }
            //    }

            //}

            //new DashboardFacade(Profile.UserName).ModifyPageLayout(setup.CurrentPage.ID, newLayout);
        }




    }

}