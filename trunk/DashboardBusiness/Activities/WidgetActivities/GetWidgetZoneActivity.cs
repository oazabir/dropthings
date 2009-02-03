namespace Dropthings.Business.Activities
{
    using System;
    using System.Collections;
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

    public partial class GetWidgetZoneActivity : System.Workflow.ComponentModel.Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for WidgetZone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidgetZoneProperty = 
            DependencyProperty.Register("WidgetZone", typeof(WidgetZone), typeof(GetWidgetZoneActivity));

        public static DependencyProperty ZoneIdProperty = DependencyProperty.Register("ZoneId", typeof(System.Int32), typeof(GetWidgetZoneActivity));

        #endregion Fields

        #region Constructors

        public GetWidgetZoneActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public WidgetZone WidgetZone
        {
            get { return (WidgetZone)GetValue(WidgetZoneProperty); }
            set { SetValue(WidgetZoneProperty, value); }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 ZoneId
        {
            get
            {
                return ((int)(base.GetValue(ZoneIdProperty)));
            }
            set
            {
                base.SetValue(ZoneIdProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var widgetZone = DatabaseHelper.GetSingle<WidgetZone, int>(DatabaseHelper.SubsystemEnum.WidgetInstance,
                this.ZoneId, LinqQueries.CompiledQuery_GetWidgetZoneById);

            this.WidgetZone = widgetZone;

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}