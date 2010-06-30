namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
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

    public partial class ChangePageLayoutActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(int), typeof(ChangePageLayoutActivity));
        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(ChangePageLayoutActivity));

        #endregion Fields

        #region Constructors

        public ChangePageLayoutActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int LayoutType
        {
            get { return (int)base.GetValue(LayoutTypeProperty); }
            set { base.SetValue(LayoutTypeProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.UpdateObject<Page, int>(DatabaseHelper.SubsystemEnum.Page,
                this.PageId, LinqQueries.CompiledQuery_GetPageById,
                (page) => {
                    page.LayoutType = this.LayoutType;
                    page.ColumnCount = Page.GetColumnWidths(this.LayoutType).Length;
                });

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}