namespace Dropthings.Business.Activities.PageActivities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
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

    public partial class DecideUniquePageNameActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for NewPageTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewPageTitleProperty = 
            DependencyProperty.Register("NewPageTitle", typeof(string), typeof(DecideUniquePageNameActivity));

        // Using a DependencyProperty as the backing store for UserGuid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserGuidProperty = 
            DependencyProperty.Register("UserGuid", typeof(Guid), typeof(DecideUniquePageNameActivity));

        #endregion Fields

        #region Constructors

        public DecideUniquePageNameActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string NewPageTitle
        {
            get { return (string)GetValue(NewPageTitleProperty); }
            set { SetValue(NewPageTitleProperty, value); }
        }

        public Guid UserGuid
        {
            get { return (Guid)GetValue(UserGuidProperty); }
            set { SetValue(UserGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            List<Page> pages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page,
                this.UserGuid, LinqQueries.CompiledQuery_GetPagesByUserId);

            string uniqueNamePrefix = "New Page";
            string pageUniqueName = uniqueNamePrefix;
            for (int counter = 0; counter < 100; counter++)
            {
                if( counter > 0 )
                    pageUniqueName = uniqueNamePrefix + " " + counter;
                if (pages.Exists((page) => page.Title == pageUniqueName) == false)
                    break;
            }

            if (pages.Exists((page) => page.Title == pageUniqueName))
                pageUniqueName = uniqueNamePrefix + "" + DateTime.Now.Ticks;

            this.NewPageTitle = pageUniqueName;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}