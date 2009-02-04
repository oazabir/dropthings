#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

namespace CustomDragDrop
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AjaxControlToolkit;

    [ClientScriptResource(null, "CustomDragDrop.CustomFloatingBehavior.js")]
    public static class CustomFloatingBehaviorScript
    {
    }

    class CustomFloatingBehaviorDesigner : AjaxControlToolkit.Design.ExtenderControlBaseDesigner<CustomFloatingBehaviorExtender>
    {
    }
}