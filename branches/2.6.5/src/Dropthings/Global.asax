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
                string fullurl = Request.Url.ToString();                
                string baseUrl = fullurl.Substring(0, fullurl.IndexOf(HttpUtility.UrlDecode(Request.Url.PathAndQuery)));

                Response.Filter = new Dropthings.Web.Util.ScriptDeferFilter(baseUrl, Response);
                
                Response.Filter = new Dropthings.Web.Util.StaticContentFilter(Response,
                    Dropthings.Util.ConstantHelper.ImagePrefix,
                    Dropthings.Util.ConstantHelper.ScriptPrefix,
                    Dropthings.Util.ConstantHelper.CssPrefix);            
            }
        }
    }


    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        
    }

    protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        var localPath = Request.Url.LocalPath;
        if (localPath.Contains(".ashx") 
            || localPath.Contains(".aspx") 
            || localPath.Contains(".axd")
            || localPath.Contains(".asmx")
            || localPath.Contains(".svc"))
        {
            // Register Facade and AppContext in HttpContext.Current.Items so that when 
            // Facade is resolved by Services.Get<>(), it returns this pre-configured facade

            var context = HttpContext.Current;
            context.Items[typeof(Dropthings.Business.Facade.Facade).FullName] = new Dropthings.Business.Facade.Facade(
                new AppContext(context, string.Empty, context.Profile.UserName));
        }
    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {
        AppContext context = AppContext.GetContext(Context);
        if (null != context)
            context.Dispose();

        foreach (object obj in Context.Items)
        {
            if (obj is IDisposable)
            {
                (obj as IDisposable).Dispose();
            }
        }
    }
</script>

