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

    public partial class GetUserGuidActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        private static DependencyProperty UserGuidProperty = DependencyProperty.Register("UserGuid", typeof(Guid), typeof(GetUserGuidActivity));
        private static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(GetUserGuidActivity));

        #endregion Fields

        #region Constructors

        public GetUserGuidActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Guid UserGuid
        {
            get { return (Guid)base.GetValue(UserGuidProperty); }
            set { base.SetValue(UserGuidProperty, value); }
        }

        [ValidationOptionAttribute(ValidationOption.Required)]
        [Browsable(true)]
        public string UserName
        {
            get { return (string)base.GetValue(UserNameProperty); }
            set { base.SetValue(UserNameProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            using( new TimedLog(UserName, "Activity: Get User Guid") )
            {
                this.UserGuid = DatabaseHelper.GetSingle<Guid, string>(DatabaseHelper.SubsystemEnum.User,
                    this.UserName, LinqQueries.CompiledQuery_GetUserGuidFromUserName);

                return ActivityExecutionStatus.Closed;
            }
        }

        #endregion Methods
    }
}