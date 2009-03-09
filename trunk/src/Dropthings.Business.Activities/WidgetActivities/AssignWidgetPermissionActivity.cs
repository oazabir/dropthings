#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data.Linq;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class AssignWidgetPermissionActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty WidgetPermissionsProperty = DependencyProperty.Register("WidgetPermissions", typeof(string), typeof(AssignWidgetPermissionActivity));

        #endregion Fields

        #region Constructors

        public AssignWidgetPermissionActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string WidgetPermissions
        {
            get { return (string)base.GetValue(WidgetPermissionsProperty); }
            set { base.SetValue(WidgetPermissionsProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            //var widget = DatabaseHelper.GetSingle<Widget, int>(DatabaseHelper.SubsystemEnum.Widget,
            //    this.WidgetId, LinqQueries.CompiledQuery_GetWidgetById);

            //widget.RoleName = this.RoleName;

            //DatabaseHelper.UpdateObject<Widget>(DatabaseHelper.SubsystemEnum.Widget,
            //    widget, null);

            var roles = DatabaseHelper.GetList<aspnet_Role>(DatabaseHelper.SubsystemEnum.Widget,
                                    LinqQueries.CompiledQuery_GetAllRole);

            if (null != this.WidgetPermissions)
            {
                // Split into category/value pairs
                foreach (string widgetPermission in this.WidgetPermissions.Split(';'))
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

                        var existingWidgetsInRoles = DatabaseHelper.GetList<WidgetsInRole, int>(DatabaseHelper.SubsystemEnum.Widget,
                                    WidgetId,
                                    LinqQueries.CompiledQuery_GetWidgetsInRoleByWidgetId);

                        foreach (aspnet_Role role in roles)
                        {
                            bool isEnable = incomingRoles != null && incomingRoles.Contains(role.RoleName);
                            var existingWidgetsInRole = existingWidgetsInRoles.Where(wir => wir.RoleId == role.RoleId).SingleOrDefault();
                            if (isEnable && existingWidgetsInRole == null)
                            {
                                DatabaseHelper.Insert<WidgetsInRole>(DatabaseHelper.SubsystemEnum.Widget, (wr) =>
                                {
                                    wr.RoleId = role.RoleId;
                                    wr.WidgetId = WidgetId;
                                });
                            }
                            else if ((!isEnable) && existingWidgetsInRole != null)
                            {

                                var widgetInstances = DatabaseHelper.GetList<WidgetInstance, int, Guid>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                                    existingWidgetsInRole.WidgetId, existingWidgetsInRole.RoleId, LinqQueries.CompiledQuery_GetAllWidgetInstancesByWidgetAndRole);

                                foreach(var widgetInstance in widgetInstances)
                                {
                                    var widgetInstanceForOtherRole = DatabaseHelper.GetList<int, int, Guid>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                                        widgetInstance.Id, existingWidgetsInRole.RoleId, LinqQueries.CompiledQuery_GetWidgetInstancesByRole);

                                    if (widgetInstanceForOtherRole == null || widgetInstanceForOtherRole.Count == 0)
                                    {
                                        DatabaseHelper.Delete<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance, widgetInstance);

                                        var list = DatabaseHelper.GetList<WidgetInstance, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                                            widgetInstance.WidgetZoneId, LinqQueries.CompiledQuery_GetWidgetInstancesByWidgetZoneId);

                                        int orderNo = 0;
                                        foreach (WidgetInstance wi in list)
                                        {
                                            wi.OrderNo = orderNo++;
                                        }

                                        DatabaseHelper.UpdateList<WidgetInstance>(DatabaseHelper.SubsystemEnum.WidgetInstance, list, null, null);
                                    }
                                }

                                DatabaseHelper.Delete<WidgetsInRole>(DatabaseHelper.SubsystemEnum.Widget, existingWidgetsInRole);

                            }
                        }
                    }
                }
            }
            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}