namespace Dropthings.Business.Facade
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Web.Security;
    using Dropthings.Data;
    using Dropthings.Model;
    using System.Configuration;
    using Dropthings.Configuration;
    using Dropthings.Util;
    using OmarALZabir.AspectF;

    partial class Facade
    {
        #region Methods

        public void SetUserRoles(string userName, string[] roles)
        {            
            for (int i = 0; i < roles.Length; i++)
            {
                if (!System.Web.Security.Roles.IsUserInRole(userName, roles[i]))
                {
                    System.Web.Security.Roles.AddUserToRole(userName, roles[i]);
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
                        string roleName = roles[i];
                        if (!System.Web.Security.Roles.RoleExists(roles[i]))
                        {
                            if (!Roles.RoleExists(roleName))
                            {
                                Roles.CreateRole(roleName);

                                var AspNetRole = this.roleRepository.GetRoleByRoleName(roles[i]);
                                var defaultWidgets = this.widgetRepository.GetWidgetByIsDefault(true);
                                    
                                if (defaultWidgets != null && defaultWidgets.Count > 0)
                                {
                                    foreach (var defaultWidget in defaultWidgets)
                                    {
                                        this.widgetsInRolesRepository.Insert(new WidgetsInRoles
                                           {
                                               Widget = new Widget { ID = defaultWidget.ID },
                                               AspNetRole = new AspNetRole { RoleId = AspNetRole.RoleId }
                                           });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string CreateUserToken(Guid userGuid, string userName)
        {
            var token = new Token
            {
                AspNetUser = new AspNetUser { UserId = userGuid },
                UserName = userName,
                UniqueID = Guid.NewGuid()
            };
            var insertedToken = this.tokenRepository.Insert(token);

            ShortGuid shortGuid = insertedToken.UniqueID;

            MembershipUser newUser = this.GetUser(userGuid);
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
                var user = this.GetUser(token.UserName);

                if (user != null)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    this.tokenRepository.Delete(token);
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
                var user = this.GetUser(userName);
                newPassword = user.ResetPassword();
            }

            return newPassword;
        }

        public void AddUserToRoleTemplate(Guid userGuid, string templateRoleName)
        {
            var RoleTemplates = this.roleTemplateRepository.GeAllRoleTemplates();

            int priority = RoleTemplates.Count == 0 ? 0 : RoleTemplates.Max(r => r.Priority);

            var AspNetRole = this.roleRepository.GetRoleByRoleName(templateRoleName);

            var RoleTemplate = this.roleTemplateRepository.GetRoleTemplateByRoleName(templateRoleName);
            
            if (RoleTemplate == null)
            {
                var insertedRoleTemplate = this.roleTemplateRepository.Insert(new RoleTemplate
                {
                    AspNetRole = new AspNetRole { RoleId = AspNetRole.RoleId },
                    AspNetUser = new AspNetUser { UserId = userGuid },
                    Priority = priority + 1
                });
            }
        }

        public RegisterUserResponse RegisterUser(string registeredUserName, string password, string email, bool isActivationRequired)
        {
            RegisterUserResponse registerUserResponse = null;

            var userGuid = CreateUser(registeredUserName, password, email);
            var userSettingTemplate = GetUserSettingTemplate();
            SetUserRoles(registeredUserName, new string[] { userSettingTemplate.RegisteredUserSettingTemplate.RoleNames });

            if (userSettingTemplate.CloneRegisteredProfileEnabled)
            {
                // Get the template user so that its page setup can be cloned for new user
                var roleTemplate = GetRoleTemplate(userGuid);

                if (!roleTemplate.AspNetUser.UserId.IsEmpty())
                {
                    // Get template user pages so that it can be cloned for new user
                    var templateUserPages = this.GetPagesOfUser(roleTemplate.AspNetUser.UserId);
                    foreach (Page templatePage in templateUserPages)
                    {
                        if (!templatePage.IsLocked)
                        {
                            ClonePage(userGuid, templatePage);
                        }
                    }
                }
            }
            else
            {
                var AspNetUser = this.userRepository.GetUserFromUserName(Context.CurrentUserName);
                TransferOwnership(userGuid, AspNetUser.UserId);
            }

            registerUserResponse = new RegisterUserResponse();

            if (isActivationRequired)
            {
                var token = CreateUserToken(userGuid, registeredUserName);
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
                var ownerName = this.pageRepository.GetPageOwnerName(pageId);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the page {1}", Context.CurrentUserName, pageId));
            }
            else if (widgetZoneId > 0)
            {
                var ownerName = this.widgetZoneRepository.GetWidgetZoneOwnerName(widgetZoneId);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the widget zone {1}", Context.CurrentUserName, widgetInstanceId));
            }

            else if (widgetInstanceId > 0)
            {
                // Get the user who is the owner of the widget. Then see if the current user is the same
                var ownerName = this.widgetInstanceRepository.GetWidgetInstanceOwnerName(widgetInstanceId);

                if (!Context.CurrentUserName.ToLower().Equals(ownerName))
                    throw new ApplicationException(string.Format("User {0} is not the owner of the widget instance {1}", Context.CurrentUserName, widgetInstanceId));
            }
        }

        public void AssignWidgetRoles(int widgetId, string[] roleNames)
        {
            var existingRoles = this.widgetsInRolesRepository.GetWidgetsInRolesByWidgetId(widgetId);
            var roles = this.roleRepository.GetAllRole();
            roleNames = roleNames.Select(r => r.ToLower()).ToArray();
            roles.Each(role =>
                {
                    var existingRole = existingRoles.FirstOrDefault(wr => wr.AspNetRole.RoleId == role.RoleId);
                    if (roleNames.Contains(role.RoleName.ToLower()))
                    {
                        // Need to give this role, if not already exists
                        if (existingRole == default(WidgetsInRoles))
                            this.widgetsInRolesRepository.Insert(new WidgetsInRoles
                                {
                                    AspNetRole = new AspNetRole { RoleId = role.RoleId },
                                    Widget = new Widget { ID = widgetId }
                                });
                    }
                    else
                    {
                        // Need to remove this role, if already exists
                        if (existingRole != default(WidgetsInRoles))
                        {
                            this.widgetsInRolesRepository.Delete(existingRole);
                        }
                    }
                });            
        }

        //public void AssignWidgetPermission(string permissions)
        //{
        //    var roles = this.roleRepository.GetAllRole();

        //    if (!string.IsNullOrEmpty(permissions))
        //    {
        //        // Split into category/value pairs
        //        foreach (string widgetPermission in permissions.Split(';'))
        //        {
        //            // Split into category and value
        //            string[] widgetPermissionPair = widgetPermission.Split(':');
        //            if (2 == widgetPermissionPair.Length)
        //            {
        //                int WidgetId = Convert.ToInt32(widgetPermissionPair[0]);
        //                string RoleNames = widgetPermissionPair[1];
        //                string[] incomingRoles = null;

        //                if (!string.IsNullOrEmpty(RoleNames))
        //                {
        //                    incomingRoles = RoleNames.Split(new char[] { ',' });
        //                }

        //                var existingWidgetsInRoles = this.widgetsInRolesRepository.GetWidgetsInRoleByWidgetId(WidgetId);

        //                foreach (AspNetRole role in roles)
        //                {
        //                    bool isEnable = incomingRoles != null && incomingRoles.Contains(role.RoleName);
        //                    var existingWidgetsInRole = existingWidgetsInRoles.Where(wir => wir.RoleId == role.RoleId).SingleOrDefault();
        //                    if (isEnable && existingWidgetsInRole == null)
        //                    {
        //                        this.widgetsInRolesRepository.Insert((wr) =>
        //                        {
        //                            wr.RoleId = role.RoleId;
        //                            wr.WidgetId = WidgetId;
        //                        });

        //                        CacheKeys.WidgetKeys.AllWidgetsKeys().Each(key => Services.Get<ICache>().Remove(key));
        //                    }
        //                    else if ((!isEnable) && existingWidgetsInRole != null)
        //                    {
        //                        // OMAR: This is going to bring the site down. This call will return
        //                        // a very large number of widget instance, may be all of them from database.
        //                        // This needs to be done entirely from a stored procedure and the SP needs
        //                        // to do the operation in small batches to prevent database locks.
        //                        var widgetInstances = this.widgetInstanceRepository.GetWidgetInstancesByWidgetAndRole(existingWidgetsInRole.WidgetId, existingWidgetsInRole.RoleId);

        //                        foreach (var widgetInstance in widgetInstances)
        //                        {
        //                            var widgetInstanceForOtherRole = this.widgetInstanceRepository.GetWidgetInstancesByRole(widgetInstance.Id, existingWidgetsInRole.RoleId);

        //                            if (widgetInstanceForOtherRole == null || widgetInstanceForOtherRole.Count == 0)
        //                            {
        //                                this.DeleteWidgetInstance(widgetInstance.Id);
        //                            }
        //                        }

        //                        this.widgetsInRolesRepository.Delete(existingWidgetsInRole);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion Methods
    }
}