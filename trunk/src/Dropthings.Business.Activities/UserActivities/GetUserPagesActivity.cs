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

    public partial class GetUserPagesActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(List<Page>), typeof(GetUserPagesActivity));
        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserPagesActivity));

        #endregion Fields

        #region Constructors

        public GetUserPagesActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public List<Page> Pages
        {
            get { return (List<Page>)base.GetValue(PagesProperty); }
            set { base.SetValue(PagesProperty, value); }
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
            using( new TimedLog(UserGuid.ToString(), "Activity: Get User Pages") )
            {
                this.Pages = DatabaseHelper.GetList<Page, Guid>(DatabaseHelper.SubsystemEnum.Page,
                    this.UserGuid, LinqQueries.CompiledQuery_GetPagesByUserId);
                return ActivityExecutionStatus.Closed;
            }
        }

        #endregion Methods
    }
}