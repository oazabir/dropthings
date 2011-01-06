using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Dropthings.Data;
using System.Data;
using System.Web.Profile;
using Dropthings.Web.Framework;
using System.Web.Security;
using Dropthings.Util;
using System.Net;
using Dropthings.Business.Facade;
using System.Net.Mail;

public partial class Setup_Default : System.Web.UI.Page
{    

    private int CurrentTaskNo
    {
        get
        {
            return int.Parse(ViewState["CurrentTaskNo"] as string ?? "0");
        }
        set
        {
            ViewState["CurrentTaskNo"] = value.ToString();
        }
    }

    private Action[] _Tasks;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this._Tasks = new Action[] 
        {
            () => TestConnectionString(),
            () => TestMembershipAPI(),
            () => TestWrite(),
            () => TestPaths(),
            () => TestUrls(),
            () => TestSMTP(),
            () => TestAppSettings(),
            () => TestAnonTemplateUser(),
            () => TestRegTemplateUser()
        };
    }

    #region Framework

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.CurrentTaskNo = 0;            
        }
    }

   
    private void MarkAsFail(Label label, string reason, string suggestion)
    {
        label.CssClass = "fail";
        label.Text += "<br /><span class=\"error\">" + HttpUtility.HtmlEncode(reason) + "</span>" +
            "<br /><span class=\"suggestion\">" + HttpUtility.HtmlEncode(suggestion) + "</span>";
    }

    private void MarkAsWarning(Label label, string suggestion)
    {
        label.CssClass = "warning";
        label.Text += "<br /><span class=\"suggestion\">" + HttpUtility.HtmlEncode(suggestion) + "</span>";
    }

    private void MarkAsPass(Label label)
    {
        label.CssClass = "pass";
    }

    protected void RefreshTimer_Tick(object sender, EventArgs e)
    {
        this._Tasks[this.CurrentTaskNo]();
        
        this.CurrentTaskNo++;
        if (this.CurrentTaskNo == this._Tasks.Length)
        {
            this.RefreshTimer.Enabled = false;
        }
    }

    #endregion

    #region Tasks

    private void TestConnectionString()
    {
        try
        {
            new DropthingsDataContext2().Test();

            MarkAsPass(ConnectionStringStatusLabel);
        }
        catch (Exception x)
        {
            MarkAsFail(ConnectionStringStatusLabel, x.Message, 
                "Most likely incorrect connection string or the connection string is not in the format Entity Framework expects. Remember EF has a special format.");
        }
    }

    private void TestMembershipAPI()
    {
        try
        {
            // Test Membership API
            try
            {
                var newUser = Membership.CreateUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
            catch (Exception x)
            {
                MarkAsFail(MembershipLabel, x.Message, 
                    "Probably wrong connection string name in <membership> block in web.config. Don't use the Entity Framework Connection string");

                return;
            }

            // Test Profile API
            try
            {
                UserProfile newProfile = ProfileCommon.Create(Guid.NewGuid().ToString()) as UserProfile;
                if (newProfile == null)
                {
                    MarkAsFail(MembershipLabel, "Cannot create new user.",
                        "You might have wrong connection string name in <profile> block. Ensure it's the simple connection string, not the entity framework one. Or you may not have the 'inherits=\"Dropthings.Web.Framework.UserProfile\" attribute in the <profile> block.");
                    return;
                }

                newProfile.IsFirstVisit = false;
                newProfile.Fullname = "Test";
                newProfile.Save();
            }
            catch (Exception x)
            {
                MarkAsFail(MembershipLabel, x.Message,
                    "Probably wrong connection string name in <profile> block in web.config. Don't use the Entity Framework Connection string");
                return;
            }

            // Test Role Manager API
            try
            {
                string[] roles = Roles.GetAllRoles();
                if (!roles.Contains("Guest"))
                {
                    MarkAsFail(MembershipLabel, "Guest role does not exist",
                    "You may have an empty database. Dropthings requires some mandetory stuff to be in some tables. There must be a Guest role.");
                    return;
                }
            }
            catch (Exception x)
            {
                MarkAsFail(MembershipLabel, x.Message,
                    "Probably wrong connection string name in <roleManager> block in web.config. Don't use the Entity Framework Connection string");
                return;
            }


            // ALL OK!
            MarkAsPass(MembershipLabel);
        }
        catch (Exception x)
        {
            MarkAsFail(MembershipLabel, x.Message, "Membership settings in web.config is incorrect. Please fix");
        }        
    }

    private void TestPaths()
    {
        bool allPathOK = true;
        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (value.StartsWith("~/"))
            {
                string fullPath = Server.MapPath(value);
                if (!Directory.Exists(fullPath) && !File.Exists(fullPath))
                {
                    MarkAsFail(FilePathLabel, "Invalid path: " + key + "=" + fullPath, string.Empty);
                    allPathOK = false;
                }
            }
        }

        if (allPathOK)
            MarkAsPass(FilePathLabel);
        
    }

    private void TestUrls()
    {
        bool allUrlOK = true;

        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            string value = ConfigurationManager.AppSettings[key];
            Uri uri;
            if (Uri.TryCreate(value, UriKind.Absolute, out uri))
            {
                // Got an URI, try hitting
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadString(uri);
                    }
                    catch (Exception x)
                    {
                        MarkAsFail(URLReachableLabel, x.Message, "Unreachable URL: " + key + "=" + uri.ToString());
                        allUrlOK = false;
                    }
                }
            }            
        }

        if (allUrlOK)
            MarkAsPass(URLReachableLabel);
    }

    private void TestAppSettings()
    {
        string fullurl = Request.Url.ToString().ToLower();                
        string baseUrl = fullurl.Substring(0, fullurl.IndexOf("setup/default.aspx")).TrimEnd('/');

        if (ConstantHelper.DeveloperMode)
            MarkAsWarning(DeveloperModeLabel, 
                "Developer mode turns off all caching and causes poor performance. It's made for developers to test changes without requiring to clear browser cache repeatedly.");
        else
            MarkAsPass(DeveloperModeLabel);

        if (baseUrl != ConstantHelper.WebRoot.TrimEnd('/'))
            MarkAsWarning(WebRootLabel, "WebRoot does not match with the current URL. WebRoot should be: " + baseUrl);
        else
            MarkAsPass(WebRootLabel);

        TestPrefix(ConstantHelper.WebRoot, ConstantHelper.WebRoot.TrimEnd('/') + "/API/Proxy.svc/ajax/js", WebServiceProxyLabel,
            "Make sure you have installed WCF REST Starter kit Preview 2 and inside web.config <baseAddresses> inside each <service> node under <system.serviceModel> has " + ConstantHelper.WebRoot);

        if (ConstantHelper.DisableDOSCheck)
            MarkAsWarning(DisableDOSCheckLabel, "It should be false on production to prevent DOS attacks.");
        else
            MarkAsPass(DisableDOSCheckLabel);

        if (ConstantHelper.DisableCache)
            MarkAsWarning(DisableCacheLabel, "It should be false on production for acceptable performance.");
        else
            MarkAsPass(DisableCacheLabel);

        TestPrefix(ConstantHelper.CssPrefix, ConstantHelper.CssPrefix + "CssHandler.ashx", CssPrefixLabel, 
            "Set CssPrefix to either empty or set to some host address from where CssHandler.ashx will be hit. Tried hitting: {0}. ");

        TestPrefix(ConstantHelper.ScriptPrefix, ConstantHelper.ScriptPrefix + "Scripts.ashx", JSPrefixLabel,
            "Set JsPrefix to either empty or set to some host address from where Scripts.ashx will be hit. Tried hitting: {0}");

        TestPrefix(ConstantHelper.ScriptPrefix, ConstantHelper.ScriptPrefix + "Scripts/MyFramework.js", JSPrefixLabel,
            "Set JsPrefix to either empty or set to some host address from where Scripts/MyFramework.js will be hit. Tried hitting: {0}");

        TestPrefix(ConstantHelper.ImagePrefix, ConstantHelper.ImagePrefix + "App_Themes/" + Page.Theme + "/StyleSheet.css", ImgPrefixLabel,
            "Set ImagePrefix to either empty or set to some host address from where theme CSS and Images will be loaded. Tried hitting: {0}");
    }

    private void TestPrefix(string constantValue, string urlToHit, Label label, string suggestion)
    {
        try
        {
            if (!string.IsNullOrEmpty(constantValue))
                using (WebClient client = new WebClient())
                    client.DownloadData(urlToHit);

            MarkAsPass(label);
        }
        catch (Exception x)
        {
            MarkAsFail(label, x.Message, suggestion.FormatWith(urlToHit));
        }
    }

    private void TestAnonTemplateUser()
    {
        var facade = Services.Get<Facade>();
        var userSettingTemplate = facade.GetUserSettingTemplate();
        var anonUserName = userSettingTemplate.AnonUserSettingTemplate.UserName;
        try
        {
            var anonUserId = facade.GetUserGuidFromUserName(anonUserName);
            var pages = facade.GetTabsOfUser(anonUserId);
            if (pages == null || pages.Count == 0)
                throw new ApplicationException("There's no pages configured for anon user template");

            MarkAsPass(AnonUserLabel);
        }
        catch (Exception x)
        {
            MarkAsFail(AnonUserLabel, x.Message, 
                "Maybe you don't have {0} properly configured." + anonUserName);
        }
    }

    private void TestRegTemplateUser()
    {
        var facade = Services.Get<Facade>();
        var userSettingTemplate = facade.GetUserSettingTemplate();
        var regUserName = userSettingTemplate.RegisteredUserSettingTemplate.UserName;
        try
        {
            var anonUserId = facade.GetUserGuidFromUserName(regUserName);
            var pages = facade.GetTabsOfUser(anonUserId);
            if (pages == null || pages.Count == 0)
                throw new ApplicationException("There's no pages configured for anon user template");

            MarkAsPass(RegisteredUserLabel);
        }
        catch (Exception x)
        {
            MarkAsFail(RegisteredUserLabel, x.Message, 
                "Maybe you don't have {0} properly configured." + regUserName);
        }
    }

    private void TestSMTP()
    {
        SmtpClient client = new SmtpClient();
        try
        {
            client.Send(ConstantHelper.AdminEmail, ConstantHelper.AdminEmail, "Test email", "Test body");
            MarkAsPass(SMTPLabel);
        }
        catch (Exception x)
        {
            MarkAsWarning(SMTPLabel, x.Message + 
                Environment.NewLine + 
                "Maybe you haven't turned on SMTP service or haven't configured Relay settings properly. You can still run Dropthings without it. But it won't be able to send emails.");
        }
    }

    private void TestWrite()
    {
        try
        {
            File.AppendAllText(Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString()), Guid.NewGuid().ToString());

            MarkAsPass(AppDataLabel);
        }
        catch (Exception x)
        {
            MarkAsFail(AppDataLabel, x.Message, "Give read, write, modify permission to NETWORK SERVICE account to App_Data folder.");
        }
    }

    protected void YesButton_Clicked(object sender, EventArgs e)
    {
        ConstantHelper.SetupCompleted = true;

        File.AppendAllText(Server.MapPath(ConstantHelper.SETUP_COMPLETE_FILE), DateTime.Now.Ticks.ToString());
        Response.Redirect("~/Default.aspx");
    }

    protected void NoButton_Clicked(object sender, EventArgs e)
    {
        
    }

    #endregion
}