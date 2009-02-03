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

    public partial class DeletePageActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(int), typeof(DeletePageActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(DeletePageActivity));

        #endregion Fields

        #region Constructors

        public DeletePageActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public int PageId
        {
            get { return (int)base.GetValue(PageIdProperty); }
            set { base.SetValue(PageIdProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DatabaseHelper.DeleteByPK<Page, int>(DatabaseHelper.SubsystemEnum.Page, this.PageId);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}