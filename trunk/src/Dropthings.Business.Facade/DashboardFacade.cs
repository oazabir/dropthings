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

    using Dropthings.Business.Container;
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

        private static bool IsValidEmail(string email)
        {
            return EmailExpression.IsMatch(email);
        }

        #endregion Methods
    }
}