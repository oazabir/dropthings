namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Web.Security;
    using Dropthings.DataAccess;
    using Dropthings.Model;
    using System.Configuration;

    partial class Facade
    {
        #region Methods

        public void SetUserRoles(string userName, string roleNames)
        {
            if (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(roleNames))
            {
                string[] roles = roleNames.Split(new char[] { ',', ':' });

                for (int i = 0; i < roles.Length; i++)
                {
                    if (!System.Web.Security.Roles.IsUserInRole(userName, roles[i]))
                    {
                        System.Web.Security.Roles.AddUserToRole(userName, roles[i]);
                    }
                }
            }
        }

        public void SetupDefaultRoles()
        {
            var settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

            foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
            {
                var roleNames = setting.RoleNames;

                if (!string.IsNullOrEmpty(roleNames))
                {
                    string[] roles = roleNames.Split(new char[] { ',', ':' });

                    for (int i = 0; i < roles.Length; i++)
                    {
                        if (!System.Web.Security.Roles.RoleExists(roles[i]))
                        {
                            if (!Roles.RoleExists(roles[i]))
                            {
                                Roles.CreateRole(roles[i]);

                                var aspnet_Role = DatabaseHelper.GetSingle<aspnet_Role, string>(DatabaseHelper.SubsystemEnum.User, roles[i], LinqQueries.CompiledQuery_GetRoleByRoleName);
                                var defaultWidgets = DatabaseHelper.GetList<Widget, bool>(DatabaseHelper.SubsystemEnum.Widget,
                                    true, LinqQueries.CompiledQuery_GetWidgetByIsDefault);

                                if (defaultWidgets != null && defaultWidgets.Count > 0)
                                {
                                    foreach (var defaultWidget in defaultWidgets)
                                    {
                                        DatabaseHelper.Insert<WidgetsInRole>(DatabaseHelper.SubsystemEnum.Widget,
                                           (newWidgetsInRole) =>
                                           {
                                               newWidgetsInRole.WidgetId = defaultWidget.ID;
                                               newWidgetsInRole.RoleId = aspnet_Role.RoleId;
                                           });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string CreateUserToken(Guid userGuid, string email)
        {
            var insertedToken = this.tokenRepository.Insert((token) =>
            {
                token.UserId = userGuid;
                token.UserName = email;
                token.UniqueID = Guid.NewGuid();
            });

            ShortGuid shortGuid = insertedToken.UniqueID;

            MembershipUser newUser = Membership.GetUser(email);
            newUser.IsApproved = false;
            Membership.UpdateUser(newUser);

            return shortGuid.Value;
        }

        public Token ActivateUser(string activationKey)
        {
            var guid = ((ShortGuid)activationKey).Guid;
            var token = this.tokenRepository.GetTokenByUniqueId(guid);

            if (token != default(Token))
            {
                var user = Membership.GetUser(token.UserName);

                if (user != null)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    this.tokenRepository.Delete(token.ID);
                }
            }

            return token;
        }

        public string ResetPassword(string forgotEmail)
        {
            var newPassword = string.Empty;
            var userName = Membership.GetUserNameByEmail(forgotEmail);
            
            if (!string.IsNullOrEmpty(userName))
            {
                var user = Membership.GetUser(userName, false);
                newPassword = user.ResetPassword();
            }

            return newPassword;
        }

        public Guid CreateUser(string registeredUserName, string password, string email)
        {
            MembershipUser newUser = Membership.CreateUser(registeredUserName, password, email);
            return (Guid)newUser.ProviderUserKey;
        }

        public void AddUserToRoleTemplate(Guid userGuid, string templateRoleName)
        {
            var RoleTemplates = this.roleTemplateRepository.GeAllRoleTemplates();

            int priority = RoleTemplates.Count == 0 ? 0 : RoleTemplates.Max(r => r.Priority);

            var aspnet_Role = DatabaseHelper.GetSingle<aspnet_Role, string>(DatabaseHelper.SubsystemEnum.User,
                    templateRoleName, LinqQueries.CompiledQuery_GetRoleByRoleName);

            var RoleTemplate = DatabaseHelper.GetSingle<RoleTemplate, string>(DatabaseHelper.SubsystemEnum.User,
                templateRoleName, LinqQueries.CompiledQuery_GetRoleTemplateByRoleName);

            if (RoleTemplate == null)
            {
                var insertedRoleTemplate = DatabaseHelper.Insert<RoleTemplate>(DatabaseHelper.SubsystemEnum.User, (newRoleTemplate) =>
                {
                    newRoleTemplate.RoleId = aspnet_Role.RoleId;
                    newRoleTemplate.TemplateUserId = userGuid;
                    newRoleTemplate.Priority = priority + 1;
                });
            }
        }

        public RegisterUserResponse RegisterUser(string registeredUserName, string password, string email, bool isActivationRequired)
        {
            RegisterUserResponse registerUserResponse = null;

            var userGuid = CreateUser(registeredUserName, password, email);
            var userSettingTemplate = GetUserSettingTemplate();
            SetUserRoles(registeredUserName, userSettingTemplate.RegisteredUserSettingTemplate.RoleNames);

            if (userSettingTemplate.CloneRegisteredProfileEnabled)
            {
                // Get the template user so that its page setup can be cloned for new user
                var roleTemplate = GetRoleTemplate(userGuid);

                if (!roleTemplate.TemplateUserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    var templateUserPages = this.pageRepository.GetPagesOfUser(roleTemplate.TemplateUserId);
                    templateUserPages.Each(templatePage =>
                    {
                        if (!templatePage.IsLocked)
                        {
                            ClonePage(userGuid, templatePage);
                        }
                    });
                }
            }
            else
            {
                var aspnet_user = this.userRepository.GetUserFromUserName(Context.CurrentUserName);
                TransferOwnership(userGuid, aspnet_user.UserId);
            }

            registerUserResponse = new RegisterUserResponse();

            if (isActivationRequired)
            {
                var token = CreateUserToken(userGuid, email);
                registerUserResponse.UnlockKey = token;
            }

            return registerUserResponse;
        }

        public void EnsureOwner(int pageId, int widgetInstanceId, int widgetZoneId)
        {
            if (pageId == 0 && widgetInstanceId == 0 && widgetZoneId == 0)
            {
                throw new ApplicationException("Nothing specified to check. Must have one of these: PageID, WidgetInstanceID, WidgetZoneID");
            }

            if (pageId > 0)
            {
                // Get the user who is the owner of the page. Then see if the current user is the same
                var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.Page,
                    pageId, LinqQueries.CompiledQuery_GetPageOwnerName);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the page {1}", Context.CurrentUserName, pageId));
            }
            else if (widgetZoneId > 0)
            {
                // Get the user who is the owner of the widget. Then see if the current user is the same
                var owners = DatabaseHelper.GetList<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    widgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneOwnerName);
                var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                   widgetZoneId, LinqQueries.CompiledQuery_GetWidgetZoneOwnerName);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the widget zone {1}", Context.CurrentUserName, widgetInstanceId));
            }

            else if (widgetInstanceId > 0)
            {
                // Get the user who is the owner of the widget. Then see if the current user is the same
                var ownerName = DatabaseHelper.GetSingle<string, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                    widgetInstanceId, LinqQueries.CompiledQuery_GetWidgetInstanceOwnerName);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the widget instance {1}", Context.CurrentUserName, widgetInstanceId));
            }
        }

        public void AssignWidgetPermission(string permissions)
        {
            var roles = this.roleRepository.GetAllRole();

            if (!string.IsNullOrEmpty(permissions))
            {
                // Split into category/value pairs
                foreach (string widgetPermission in permissions.Split(';'))
                {
                    // Split into category and value
                    string[] widgetPermissionPair = widgetPermission.Split(':');
                    if (2 == widgetPermissionPair.Length)
                    {
                        int WidgetId = Convert.ToInt32(widgetPermissionPair[0]);
                        string RoleNames = widgetPermissionPair[1];
                        string[] incomingRoles = null;

                        if (!string.IsNullOrEmpty(RoleNames))
                        {
                            incomingRoles = RoleNames.Split(new char[] { ',' });
                        }

                        var existingWidgetsInRoles = this.widgetsInRolesRepository.GetWidgetsInRoleByWidgetId(WidgetId);

                        foreach (aspnet_Role role in roles)
                        {
                            bool isEnable = incomingRoles != null && incomingRoles.Contains(role.RoleName);
                            var existingWidgetsInRole = existingWidgetsInRoles.Where(wir => wir.RoleId == role.RoleId).SingleOrDefault();
                            if (isEnable && existingWidgetsInRole == null)
                            {
                                this.widgetsInRolesRepository.Insert((wr) =>
                                {
                                    wr.RoleId = role.RoleId;
                                    wr.WidgetId = WidgetId;
                                });
                            }
                            else if ((!isEnable) && existingWidgetsInRole != null)
                            {
                                var widgetInstances = this.widgetInstanceRepository.GetWidgetInstancesByWidgetAndRole(existingWidgetsInRole.WidgetId, existingWidgetsInRole.RoleId);

                                foreach (var widgetInstance in widgetInstances)
                                {
                                    var widgetInstanceForOtherRole = this.widgetInstanceRepository.GetWidgetInstancesByRole(widgetInstance.Id, existingWidgetsInRole.RoleId);

                                    if (widgetInstanceForOtherRole == null || widgetInstanceForOtherRole.Count == 0)
                                    {
                                        this.widgetInstanceRepository.Delete(widgetInstance);

                                        var list = this.widgetInstanceRepository.GetWidgetInstancesByWidgetZoneId(widgetInstance.WidgetZoneId);

                                        int orderNo = 0;
                                        foreach (WidgetInstance wi in list)
                                        {
                                            wi.OrderNo = orderNo++;
                                        }

                                        this.widgetInstanceRepository.UpdateList(list, null, null);
                                    }
                                }

                                this.widgetsInRolesRepository.Delete(existingWidgetsInRole);
                            }
                        }
                    }
                }
            }
        }

        public void OverrideCurrentPageIfTemplateUser(string userName)
        {
            var userGuid = this.userRepository.GetUserGuidFromUserName(userName);
            var roleTemplate = GetRoleTemplate(userGuid);

            if(roleTemplate.TemplateUserId.Equals(userGuid))
            {
                //override the current page with server as start page setting
                Page overridablePage = this.pageRepository.GetOverridableStartPageOfUser(userGuid);

                if(overridablePage != null)
                {
                    //get current setting
                    var userSetting = GetUserSetting(userGuid);

                    if (userSetting.CurrentPageId != overridablePage.ID)
                    {
                        this.userSettingRepository.Update(userSetting, (setting) =>
                        {
                            setting.CurrentPageId = overridablePage.ID;
                        }, null);
                    }
                }
            }
        }

        #endregion Methods
    }
}