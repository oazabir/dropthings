#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for IWidget
/// </summary>
namespace Dropthings.Widget.Framework
{
    using System;

    using Dropthings.DataAccess;

    public interface IWidget : IEventListener
    {
        #region Methods

        void Closed();

        void Collasped();

        void Expanded();

        void HideSettings();

        void Init(IWidgetHost host);

        void Maximized();

        void Restored();

        void ShowSettings();

        #endregion Methods
    }
}