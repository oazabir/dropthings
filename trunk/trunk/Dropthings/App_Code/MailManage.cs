// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.IO;
/// <summary>
/// Summary description for MailManager
/// </summary>
namespace Dropthings.Web.Util
{
    public static class MailManager
    {
        private static readonly string adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
        private static readonly string webRoot = ConfigurationManager.AppSettings["WebRoot"];
        
        private static void SendMail(string to, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(adminEmail);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                var mailClient = new SmtpClient();
                mailClient.Send(message);
            }
        }

        public static void SendSignupMail(string email, string password, bool activationRequired, string unlockKey)
        {
            const string key = "signup";

            var body = HttpContext.Current.Cache[key] as string;

            if (body == null)
            {
                var file = System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/Signup.txt");
                body = File.ReadAllText(file);

                HttpContext.Current.Cache[key] = body;
            }

            body = body.Replace("<%Email%>", email);
            body = body.Replace("<%Password%>", password);

            if (activationRequired)
            {
                string done = string.Format("Please <a href={0}Verify.ashx?key={1}>click here</a> to activate your account.", webRoot, unlockKey);
                body = body.Replace("<%ActivateAccount%>", done);
            }
            else
            {
                body = body.Replace("<%ActivateAccount%>", string.Empty);
            }

            SendMail(email, "Dropthings: New Account Created.", body);
        }

        public static void SendPasswordMail(string email, string password)
        {
            const string key = "password";

            var body = HttpContext.Current.Cache[key] as string;

            if (string.IsNullOrEmpty(body))
            {
                var file = System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/Password.txt");
                body = File.ReadAllText(file);

                HttpContext.Current.Cache[key] = body;
            }

            body = body.Replace("<%Password%>", password);

            SendMail(email, "Dropthings: New Password", body);
        }
    }
}