#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Transactions;
    using System.Web.Security;
    using System.Workflow.Runtime;

    using Dropthings.Business.Container;
    using Dropthings.Business.Workflows;
    using Dropthings.Business.Workflows.EntryPointWorkflows;
    using Dropthings.Business.Workflows.TabWorkflows;
    using Dropthings.Business.Workflows.UserAccountWorkflow;
    using Dropthings.Business.Workflows.UserAccountWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows;
    using Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs;
    using Dropthings.DataAccess;

    public class DashboardFacade
    {
        #region Fields

        private static readonly Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);

        private string _UserName;

        #endregion Fields

        #region Constructors

        public DashboardFacade(string userName)
        {
            this._UserName = userName;
        }

        #endregion Constructors

        #region Methods

        public Token ActivateAccount(string activationKey)
        {
            using (new TimedLog(this._UserName, "Verify Account"))
            {
                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ActivateAccountWorkflow,
                        ActivateAccountWorkflowRequest,
                        ActivateAccountWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ActivateAccountWorkflowRequest { ActivationKey = activationKey, UserName = this._UserName }
                        );

                return response.Token;
            }
        }

        public AddNewTabWorkflowResponse AddNewPage(string layoutType)
        {
            using (new TimedLog(this._UserName, "Add New Page"))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.LAYOUT_TYPE, layoutType);

                //// NewPage will be returned after workflow completes
                //properties.Add(WorkflowConstants.NEW_PAGE, null);

                //ObjectContainer.Resolve<IWorkflowHelper>()
                //    .ExecuteWorkflow(
                //        ObjectContainer.Resolve<WorkflowRuntime>(),
                //        typeof(AddNewTabWorkflow), properties
                //    );

                //return properties[WorkflowConstants.NEW_PAGE] as Page;

                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        AddNewTabWorkflow,
                        AddNewTabWorkflowRequest,
                        AddNewTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new AddNewTabWorkflowRequest { LayoutType = layoutType, UserName = this._UserName }
                        );

                return response;
            }
        }

        public WidgetInstance AddWidget(int widgetId, int column, int toRow, int zoneId)
        {
            using (new TimedLog(this._UserName, "Add Widget" + widgetId))
            {
                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        AddWidgetWorkflow,
                        AddWidgetRequest,
                        AddWidgetResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new AddWidgetRequest { WidgetId = widgetId, RowNo = toRow, ColumnNo = column, ZoneId = zoneId, UserName = this._UserName }
                        );

                return response.NewWidget;
            }
        }

        public void ChangeCurrentTab(int pageID)
        {
            using (new TimedLog(this._UserName, "ChangeCurrentTab"))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.PAGE_ID, pageID);

                //ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(ObjectContainer.Resolve<WorkflowRuntime>(),typeof(ChangeTabWorkflow), properties);

                ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ChangeTabWorkflow,
                        ChangeTabWorkflowRequest,
                        ChangeTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ChangeTabWorkflowRequest { PageID = pageID, UserName = this._UserName }
                        );
            }
        }

        public void ChangePageName(string newName)
        {
            using (new TimedLog(this._UserName, "Change Page Name"))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.PAGE_NAME, newName);

                //ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(ObjectContainer.Resolve<WorkflowRuntime>(),typeof(ChangePageNameWorkflow), properties);

                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ChangePageNameWorkflow,
                        ChangeTabNameWorkflowRequest,
                        ChangeTabNameWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ChangeTabNameWorkflowRequest { PageName = newName, UserName = this._UserName }
                        );
            }
        }

        public DeleteTabWorkflowResponse DeleteCurrentPage(int pageID)
        {
            using (new TimedLog(this._UserName, "DeletePage"))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.PAGE_ID, pageID);

                //// Workflow will return the current page
                //properties.Add(WorkflowConstants.NEW_CURRENT_PAGE, null);

                //ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(ObjectContainer.Resolve<WorkflowRuntime>(),typeof(DeletePageWorkflow), properties);

                //return properties[WorkflowConstants.NEW_CURRENT_PAGE] as Page;

                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeletePageWorkflow,
                        DeleteTabWorkflowRequest,
                        DeleteTabWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new DeleteTabWorkflowRequest {  PageID = pageID, UserName = this._UserName }
                        );

                return response;
            }
        }

        public void DeleteWidgetInstance(int id)
        {
            using (new TimedLog(this._UserName, "Delete Widget:" + id))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.WIDGET_INSTANCE_ID, id);
                //ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(ObjectContainer.Resolve<WorkflowRuntime>(),typeof(DeleteWidgetInstanceWorkflow), properties);

                ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        DeleteWidgetInstanceWorkflow,
                        DeleteWidgetInstanceWorkflowRequest,
                        DeleteWidgetInstanceWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new DeleteWidgetInstanceWorkflowRequest { WidgetInstanceId = id, UserName = this._UserName }
                        );
            }
        }

        public void ExpanCollaspeWidgetInstance(int widgetInstanceId, bool expanded)
        {
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ExpandWidgetInstanceWorkflow,
                        ExpandWidgetInstanceRequest,
                        ExpandWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new ExpandWidgetInstanceRequest { UserName = this._UserName, WidgetInstanceId = widgetInstanceId, IsExpand = expanded }
                    );
        }

        public List<Widget> GetWidgetList(Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return DatabaseHelper.GetList<Widget, Dropthings.DataAccess.Enumerations.WidgetTypeEnum>(DatabaseHelper.SubsystemEnum.Widget, widgetType,
                LinqQueries.CompiledQuery_GetAllWidgets);
        }

        public List<Widget> GetWidgetList(string username, Dropthings.DataAccess.Enumerations.WidgetTypeEnum widgetType)
        {
            return DatabaseHelper.GetList<Widget, string, Dropthings.DataAccess.Enumerations.WidgetTypeEnum>(DatabaseHelper.SubsystemEnum.Widget, username, widgetType,
                LinqQueries.CompiledQuery_GetWidgetsByRole).Distinct().ToList();
        }

        public bool IsWidgetInRole(int widgetId, string roleName)
        {
            WidgetsInRole widgetsInRole = DatabaseHelper.GetSingle<WidgetsInRole, int, string>(DatabaseHelper.SubsystemEnum.Widget,
                widgetId, roleName, LinqQueries.CompiledQuery_GetWidgetsInRolesByRoleName);

            return widgetsInRole != null;
        }

        public UserVisitWorkflowResponse LoadUserSetup(string pageTitle)
        {
            using (new TimedLog(this._UserName, "Total: Existing user visit"))
            {
                /*
                var properties = new Dictionary<string, object>();
                properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                properties.Add(WorkflowConstants.PAGE_NAME, pageTitle);
                var userSetup = new UserPageSetup();
                properties.Add(WorkflowConstants.USER_PAGE_SETUP, userSetup);

                BusinessContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(BusinessContainer.Resolve<WorkflowRuntime>(),typeof(UserVisitWorkflow), properties);
                return userSetup;
                */

                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        UserVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new UserVisitWorkflowRequest { PageName = pageTitle, UserName = this._UserName }
                        );

                return response;
            }
        }

        public void MaximizeRestoreWidgetInstance(int widgetInstanceId, bool maximized)
        {
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        MaximizeWidgetInstanceWorkflow,
                        MaximizeWidgetInstanceRequest,
                        MaximizeWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new MaximizeWidgetInstanceRequest { UserName = this._UserName, WidgetInstanceId = widgetInstanceId, IsMaximize = maximized}
                    );
        }

        public void ModifyPageLayout(int pageID, int newLayoutType)
        {
            using (new TimedLog(this._UserName, "ModifyPageLayout"))
            {
                //var properties = new Dictionary<string, object>();
                //properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                //properties.Add(WorkflowConstants.LAYOUT_TYPE, newLayoutType);
                //properties.Add(WorkflowConstants.PAGE_ID, pageID);
                //ObjectContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(ObjectContainer.Resolve<WorkflowRuntime>(),typeof(ModifyPageLayoutWorkflow), properties);

                ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ModifyPageLayoutWorkflow,
                        ModifyTabLayoutWorkflowRequest,
                        ModifyTabLayoutWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ModifyTabLayoutWorkflowRequest { PageID = pageID, LayoutType = newLayoutType.ToString(), UserName = this._UserName }
                        );
            }
        }

        public void MoveWidgetInstance(int widgetInstanceId, int newZoneId, int rowNo)
        {
            using (new TimedLog(this._UserName, "Move Widget:" + widgetInstanceId))
            {
                ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<MoveWidgetInstanceWorkflow, MoveWidgetInstanceWorkflowRequest, MoveWidgetInstanceWorkflowResponse>(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new MoveWidgetInstanceWorkflowRequest { NewZoneId = newZoneId, RowNo = rowNo, UserName = this._UserName, WidgetInstanceId = widgetInstanceId });
            }
        }

        public UserVisitWorkflowResponse NewUserVisit()
        {
            using (new TimedLog(this._UserName, "New user visit"))
            {
                /*
                var properties = new Dictionary<string, object>();
                properties.Add(WorkflowConstants.USER_NAME, this._UserName);
                var userSetup = new UserPageSetup();
                properties.Add(WorkflowConstants.USER_PAGE_SETUP, userSetup);

                BusinessContainer.Resolve<IWorkflowHelper>().ExecuteWorkflow(BusinessContainer.Resolve<WorkflowRuntime>(),typeof(FirstVisitWorkflow), properties);
                return userSetup;
                */
                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        FirstVisitWorkflow,
                        UserVisitWorkflowRequest,
                        UserVisitWorkflowResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new UserVisitWorkflowRequest { PageName = string.Empty, UserName = this._UserName }
                    );
                return response;
            }
        }

        public void RegisterAs(string email, string password, bool activationRequired)
        {
            using (new TimedLog(this._UserName, "Register As: " + email))
            {
                var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        UserRegistrationWorkflow,
                        UserRegistrationWorkflowRequest,
                        UserRegistrationWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new UserRegistrationWorkflowRequest { Email = email, Password = password, IsActivationRequired = activationRequired, UserName = this._UserName }
                        );

                //unlockKey = string.Empty;
                //MembershipUser newUser = Membership.GetUser(email);

                //// Get the User Id for the anonymous user from the aspnet_users table
                //aspnet_User anonUser = DatabaseHelper.GetSingle<aspnet_User, string>(DatabaseHelper.SubsystemEnum.User,
                //    this._UserName, LinqQueries.CompiledQuery_GetUserFromUserName);

                //Guid oldGuid = anonUser.UserId;
                //Guid newGuid = (Guid)newUser.ProviderUserKey;

                //// Move page ownership
                //using (TransactionScope ts = new TransactionScope())
                //{
                //    List<Page> pages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page,
                //        oldGuid, LinqQueries.CompiledQuery_GetPagesByUserId);

                //    foreach (Page page in pages)
                //    {
                //        page.UserId = newGuid;
                //    }
                //    // Delete setting for the anonymous user and create new setting for the new user
                //    UserSetting setting = DatabaseHelper.GetSingle<UserSetting, Guid>(DatabaseHelper.SubsystemEnum.User,
                //        oldGuid, LinqQueries.CompiledQuery_GetUserSettingByUserGuid);
                //    DatabaseHelper.Delete<UserSetting>(DatabaseHelper.SubsystemEnum.User, setting);
                //    DatabaseHelper.Insert<UserSetting>(DatabaseHelper.SubsystemEnum.User, (newSetting) =>
                //        {
                //            newSetting.UserId = newGuid;
                //            newSetting.CurrentPageId = setting.CurrentPageId;
                //            newSetting.CreatedDate = DateTime.Now;
                //        });

                //    if (activationRequired)
                //    {
                //        Token insertedToken = DatabaseHelper.Insert<Token>(DatabaseHelper.SubsystemEnum.Token, (token) =>
                //            {
                //                token.UserId = newGuid;
                //                token.UserName = email;
                //                token.UniqueID = Guid.NewGuid();
                //            });

                //        ShortGuid shortGuid = insertedToken.UniqueID;
                //        unlockKey = shortGuid.Value;

                //        newUser.IsApproved = false;
                //        Membership.UpdateUser(newUser);
                //    }

                //    ts.Complete();
                //}

            }
        }

        public string ResetPassword(string email)
        {
            var response = ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ResetPasswordWorkflow,
                        ResetPasswordWorkflowRequest,
                        ResetPasswordWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new ResetPasswordWorkflowRequest { Email = email, UserName = this._UserName }
                        );

                return response.NewPassword;
        }

        public void ResizeWidgetInstance(int widgetInstanceId, int width, int height)
        {
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        ResizeWidgetInstanceWorkflow,
                        ResizeWidgetInstanceRequest,
                        ResizeWidgetInstanceResponse
                    >(
                        ObjectContainer.Resolve<WorkflowRuntime>(),
                        new ResizeWidgetInstanceRequest { UserName = this._UserName, WidgetInstanceId = widgetInstanceId, Width = width, Hidth = height }
                    );
        }

        public void UpdateAccount(string email)
        {
            ObjectContainer.Resolve<IWorkflowHelper>()
                    .ExecuteWorkflow<
                        UpdateAccountWorkflow,
                        UpdateAccountWorkflowRequest,
                        UpdateAccountWorkflowResponse
                        >(
                            ObjectContainer.Resolve<WorkflowRuntime>(),
                            new UpdateAccountWorkflowRequest { Email = email, UserName = this._UserName }
                        );
        }

        private static bool IsValidEmail(string email)
        {
            return EmailExpression.IsMatch(email);
        }

        #endregion Methods
    }
}