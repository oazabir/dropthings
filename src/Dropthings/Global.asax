<%@ Application Language="C#" %>
<%@ Import Namespace="Dropthings.Business.Facade.Context" %>

<script RunAt="server">
    // Copyright (c) Omar AL Zabir. All rights reserved.
    // For continued development and updates, visit http://msmvps.com/omar

    const string APPLICATION_WORKFLOW_RUNTIME_KEY = "GlobalSynchronousWorkflowRuntime";
    void Application_Start(object sender, EventArgs e)
    {
        Dropthings.Business.Facade.Facade.BootStrap();
    }

    void Application_End(object sender, EventArgs e)
    {
        Dropthings.Util.Services.Dispose();
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        if (null != Context && null != Context.AllErrors)
            System.Diagnostics.Debug.WriteLine(Context.AllErrors.Length);
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // Simulate internet latency on local browsing
        if (Request.IsLocal)
            System.Threading.Thread.Sleep(100);
        
        if (Request.HttpMethod == "GET")
        {
            if (Request.AppRelativeCurrentExecutionFilePath.EndsWith(".aspx"))
            {
                Response.Filter = new Dropthings.Web.Util.ScriptDeferFilter(Response);

                Response.Filter = new Dropthings.Web.Util.StaticContentFilter(Response,
                    ConfigurationManager.AppSettings["ImgPrefix"],
                    ConfigurationManager.AppSettings["JsPrefix"],
                    ConfigurationManager.AppSettings["CssPrefix"]);
            }
        }
    }


    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        
    }

    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        // setup AppContext for this http request
        var context = new AppContext(Context, string.Empty, Profile.UserName);
    }
</script>

