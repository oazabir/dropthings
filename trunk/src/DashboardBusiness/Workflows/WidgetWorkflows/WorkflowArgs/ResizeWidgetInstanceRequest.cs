namespace Dropthings.Business.Workflows.WidgetWorkflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ResizeWidgetInstanceRequest : UserWorkflowRequestBase
    {
        #region Properties

        public int Hidth
        {
            get; set;
        }

        public int WidgetInstanceId
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        #endregion Properties
    }
}