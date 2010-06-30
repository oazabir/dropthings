namespace Dropthings.Business.Activities.PageActivities
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

    public partial class MoveWidgetFromOnColumnToAnother : SequenceActivity
    {
        #region Fields

        public static DependencyProperty NewColumnNoProperty = DependencyProperty.Register("NewColumnNo", typeof(System.Int32), typeof(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother));
        public static DependencyProperty NewWidgetZoneProperty = DependencyProperty.Register("NewWidgetZone", typeof(Dropthings.DataAccess.WidgetZone), typeof(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother));
        public static DependencyProperty OldColumnoProperty = DependencyProperty.Register("OldColumno", typeof(System.Int32), typeof(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother));
        public static DependencyProperty OldWidgetZoneProperty = DependencyProperty.Register("OldWidgetZone", typeof(Dropthings.DataAccess.WidgetZone), typeof(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother));
        public static DependencyProperty PageIdProperty = DependencyProperty.Register("PageId", typeof(System.Int32), typeof(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother));

        public System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance> OldWidgetInstances = new System.Collections.Generic.List<Dropthings.DataAccess.WidgetInstance>();

        #endregion Fields

        #region Constructors

        public MoveWidgetFromOnColumnToAnother()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 NewColumnNo
        {
            get
            {
                return ((int)(base.GetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.NewColumnNoProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.NewColumnNoProperty, value);
            }
        }

        [System.ComponentModel.DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Misc")]
        public WidgetZone NewWidgetZone
        {
            get
            {
                return ((Dropthings.DataAccess.WidgetZone)(base.GetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.NewWidgetZoneProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.NewWidgetZoneProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 OldColumno
        {
            get
            {
                return ((int)(base.GetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.OldColumnoProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.OldColumnoProperty, value);
            }
        }

        [System.ComponentModel.DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Misc")]
        public WidgetZone OldWidgetZone
        {
            get
            {
                return ((Dropthings.DataAccess.WidgetZone)(base.GetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.OldWidgetZoneProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.OldWidgetZoneProperty, value);
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Int32 PageId
        {
            get
            {
                return ((int)(base.GetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.PageIdProperty)));
            }
            set
            {
                base.SetValue(Dropthings.Business.Activities.PageActivities.MoveWidgetFromOnColumnToAnother.PageIdProperty, value);
            }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            base.Execute(executionContext);
            return ActivityExecutionStatus.Closed;
        }

        private void ForEachWidgetInstanceToMove_Iterating(object sender, EventArgs e)
        {
            ForEachActivity.ForEach forEachActivity = (sender as ForEachActivity.ForEach);
            var widgetInstance = forEachActivity.Enumerator.Current as WidgetInstance;

            var moveWidgetActivity = forEachActivity.DynamicActivity as ChangeWidgetInstancePositionActivity;
            moveWidgetActivity.WidgetInstanceId = widgetInstance.Id;
        }

        #endregion Methods
    }
}