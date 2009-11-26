#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for ActionValidator
/// </summary>
namespace Dropthings.Web.Util
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public static class WebUtil
    {        
        #region Methods

        public static void ShowMessage(Label label, string message, bool isError)
        {
            label.ForeColor = System.Drawing.Color.DodgerBlue;
            label.Text = message;

            if (isError)
            {
                label.ForeColor = System.Drawing.Color.Red;
                label.Font.Bold = true;
            }
        }

        #endregion Methods
    }
}