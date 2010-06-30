#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections.Generic;
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

    public partial class CreateDeafultWidgetsOnPageActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        public static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(System.Int32), typeof(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity));
        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity));

        #endregion Fields

        #region Constructors

        public CreateDeafultWidgetsOnPageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Int32 PageId
        {
            get
            {
                return ((int)(base.GetValue(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.PageIdProperty, value);
            }
        }

        [Browsable(true)]
        public string UserName
        {
            get
            {
                return ((string)(base.GetValue(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.UserNameProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.CreateDeafultWidgetsOnPageActivity.UserNameProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            List<Widget> defaultWidgets = null;

            if (System.Web.Security.Roles.Enabled && !string.IsNullOrEmpty(UserName))
            {
               defaultWidgets = DatabaseHelper.GetList<Widget, string, Dropthings.DataAccess.Enumerations.WidgetTypeEnum, bool>(DatabaseHelper.SubsystemEnum.Widget, UserName, Dropthings.DataAccess.Enumerations.WidgetTypeEnum.PersonalPage, true,
                LinqQueries.CompiledQuery_GetDefaultWidgetsByRole).Distinct().ToList(); ;
            }
            else
            {
                defaultWidgets = DatabaseHelper.GetList<Widget, bool>(DatabaseHelper.SubsystemEnum.Widget,
                    true, LinqQueries.CompiledQuery_GetWidgetByIsDefault);
            }

            var widgetsPerColumn = (int)Math.Ceiling((float)defaultWidgets.Count / 3.0);

            var row = 0;
            var col = 0;

            DatabaseHelper.InsertList<WidgetInstance, Widget>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                defaultWidgets, (widget) =>
                    {
                        var newWidget = new WidgetInstance();

                        var widgetZone = DatabaseHelper.GetSingle<WidgetZone, int, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                            this.PageId, col, LinqQueries.CompiledQuery_GetWidgetZoneByPageId_ColumnNo);

                        newWidget.WidgetZoneId = widgetZone.ID;
                        newWidget.OrderNo = row;
                        newWidget.CreatedDate = DateTime.Now;
                        newWidget.Expanded = true;
                        newWidget.Title = widget.Name;
                        newWidget.VersionNo = 1;
                        newWidget.WidgetId = widget.ID;
                        newWidget.State = widget.DefaultState;

                        row++;
                        if (row >= widgetsPerColumn)
                        {
                            row = 0;
                            col++;
                        }

                        return newWidget;
                    });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}