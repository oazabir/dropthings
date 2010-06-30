namespace Dropthings.Business.Activities
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

    public partial class DecideCurrentPageActivity : Activity
    {
        #region Fields

        public static readonly DependencyProperty CurrentPageIdProperty = 
            DependencyProperty.Register("CurrentPageId", typeof(int), typeof(DecideCurrentPageActivity));

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty = 
            DependencyProperty.Register("CurrentPage", typeof(Page), typeof(DecideCurrentPageActivity));

        // Using a DependencyProperty as the backing store for UserGuid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserGuidProperty = 
            DependencyProperty.Register("UserGuid", typeof(Guid), typeof(DecideCurrentPageActivity));

        private static DependencyProperty PageNameProperty = 
            DependencyProperty.Register("PageName", typeof(string), typeof(DecideCurrentPageActivity));

        #endregion Fields

        #region Constructors

        public DecideCurrentPageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Page CurrentPage
        {
            get { return (Page)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public int CurrentPageId
        {
            get { return (int)GetValue(CurrentPageIdProperty); }
            set { SetValue(CurrentPageIdProperty, value); }
        }

        public string PageName
        {
            get { return (string)base.GetValue(PageNameProperty); }
            set { base.SetValue(PageNameProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)GetValue(UserGuidProperty); }
            set { SetValue(UserGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            string pageName = this.PageName;

            List<Page> userPages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page,
                this.UserGuid, LinqQueries.CompiledQuery_GetPagesByUserId);

            // Find the page that has the specified Page Name and make it as current
            // page. This is needed to make a tab as current tab when the tab name is
            // known
            if (!string.IsNullOrEmpty(pageName))
            {

                foreach (Page page in userPages)
                {
                    if (page.Title.Replace(' ', '_') == pageName)
                    {
                        this.CurrentPageId = page.ID;
                        this.CurrentPage = page;
                        break;
                    }
                }
            }

            // If there's no such page, then the first page user has will be the current
            // page. This happens when a page is deleted.
            if (this.CurrentPageId == 0)
            {
                this.CurrentPageId = userPages[0].ID;
                this.CurrentPage = userPages[0];
            }

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}