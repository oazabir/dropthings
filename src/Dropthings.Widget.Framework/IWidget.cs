// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using Dropthings.DataAccess;

/// <summary>
/// Summary description for IWidget
/// </summary>
namespace Dropthings.Widget.Framework
{
    public interface IWidget : IEventListener
    {
        void Init(IWidgetHost host);
        void ShowSettings();
        void HideSettings();
        void Expanded();
        void Collasped();
        void Maximized();
        void Restored();
        void Closed();        
    }
}