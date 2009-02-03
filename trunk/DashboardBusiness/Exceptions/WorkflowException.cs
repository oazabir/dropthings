#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace Dropthings.Business.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    class WorkflowException : ApplicationException
    {
        #region Constructors

        public WorkflowException(Exception actualException)
            : base(actualException.Message, actualException)
        {
        }

        #endregion Constructors
    }
}