#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header


[assembly: System.Web.UI.WebResource("CustomDragDrop.CustomFloatingBehavior.js", "text/javascript")]

namespace CustomDragDrop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using AjaxControlToolkit;

    [Designer(typeof(CustomFloatingBehaviorDesigner))]
    [ClientScriptResource("CustomDragDrop.CustomFloatingBehavior", "CustomDragDrop.CustomFloatingBehavior.js")]
    [TargetControlType(typeof(WebControl))]
    [RequiredScript(typeof(DragDropScripts))]
    public class CustomFloatingBehaviorExtender : ExtenderControlBase
    {
        #region Properties

        [ExtenderControlProperty]
        [IDReferenceProperty(typeof(WebControl))]
        public string DragHandleID
        {
            get
            {
                return GetPropertyValue<String>("DragHandleID", string.Empty);
            }
            set
            {
                SetPropertyValue<String>("DragHandleID", value);
            }
        }

        #endregion Properties
    }
}