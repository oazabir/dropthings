// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using Dropthings.DataAccess;

/// <summary>
/// Summary description for IWidget
/// </summary>
namespace Dropthings.Widget.Framework
{
    public interface IWidget
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


    public interface IWidgetHost
    {
        int ID { get; }
        void SaveState(string state);
        string GetState();
        void Expand();
        void Collaspe();
        void Maximize();
        void Restore();
        void Close();
        WidgetInstance WidgetInstance { get; set; }
        event Action<WidgetInstance, IWidgetHost> Deleted;
        void ShowSettings();
        void HideSettings();
    }
}