namespace Dropthings.Business.Workflows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GenericWorkflowResponeBase<T>
    {
        #region Properties

        public T Data
        {
            get; set;
        }

        #endregion Properties
    }
}