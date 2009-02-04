namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AssignWidgetPermissionRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int WidgetId
        {
            get;
            set;
        }

        public string WidgetPermissions
        {
            get; set;
        }

        #endregion Properties
    }
}