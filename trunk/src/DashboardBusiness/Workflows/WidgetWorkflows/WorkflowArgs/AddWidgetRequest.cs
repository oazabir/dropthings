namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AddWidgetRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int ColumnNo
        {
            get; set;
        }

        public int RowNo
        {
            get; set;
        }

        public int WidgetId
        {
            get; set;
        }

        public int ZoneId
        {
            get; set;
        }

        #endregion Properties
    }
}