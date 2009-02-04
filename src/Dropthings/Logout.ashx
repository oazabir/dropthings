<%@ WebHandler Language="C#" Class="Logout" %>
// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar


using System;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Dropthings.Web.Util;
public class Logout : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        /// Expire all the cookies so browser visits us as a brand new user
        CookieHelper.Clear();
        context.Response.Redirect("~/Default.aspx");        
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}