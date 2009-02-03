// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for ExtensionFunctions
/// </summary>
public static class ExtensionFunctions
{
    public static string FormatWith(this string s, params object[] args)
    {
        return string.Format(s, args);
    }
    public static string Xml(this XElement e)
    {
        StringBuilder builder = new StringBuilder();
        XmlTextWriter writer = new XmlTextWriter(new StringWriter(builder));
        e.WriteTo(writer);
        return builder.ToString();
    }

    public static string TabName(this Dropthings.DataAccess.Page p)
    {
        return p.Title.Replace(' ', '_');
    }

    public static void Times(this int count, Action<int> doSomething)
    {
        for (int i = 0; i < count; i++)
            doSomething(i);
    }

    public static void Each<T>(this T[] array, Action<T, int> act)
    {
        int counter = 0;
        foreach (T item in array)
            act(item, counter++);
    }

    public static void Each<T>(this IList<T> list, Action<T, int> act)
    {
        int counter = 0;
        foreach (T item in list)
            act(item, counter++);
    }

    public static void Each<T>(this IList<T> list, Action<T> act)
    {
        foreach (T item in list)
            act(item);
    }

    public static string Percent(this int value)
    {
        return value + "%";
    }

    public static string Pixels(this int value)
    {
        return value + "px";
    }
}
