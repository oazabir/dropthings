using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Dropthings.Web.Framework;
using Dropthings.Widget.Framework;

using Dimebrain.TweetSharp;
using Dimebrain.TweetSharp.Fluent;


/// <summary>
/// Summary description for TwitterService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class TwitterService : System.Web.Services.WebService
{
    private TwitterClientInfo info;

    public TwitterService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        info = new TwitterClientInfo() { ClientName = "Dropthings", ClientVersion = "1.0" };
    }

    [WebMethod]
    public string GetPublicStatuses()
    {
        var twitter = FluentTwitter.CreateRequest(info)
                      .Statuses()
                      .OnPublicTimeline().AsJson();

        return twitter.Request();
    }

    [WebMethod]
    public string VerifyCredentials(string username, string password)
    {
        var userCredentials = FluentTwitter.CreateRequest(info)
                              .AuthenticateAs(username,password)
                              .Accounts().VerifyCredentials().AsJson();
        return userCredentials.Request();
    }

    [WebMethod]
    public string GetUserStatuses(string username, string password)
    {
        var userStatus = FluentTwitter.CreateRequest(info)
                         .AuthenticateAs(username, password)
                         .Statuses()
                         .OnUserTimeline().AsJson();
        return userStatus.Request();
    }

    [WebMethod]
    public string GetFriendStatuses(string username, string password)
    {
        var friendStatus = FluentTwitter.CreateRequest(info)
                         .AuthenticateAs(username, password)
                         .Statuses()
                         .OnFriendsTimeline().AsJson();
        return friendStatus.Request();
    }

    [WebMethod]
    public string UpdateStaus(string username, string password, string updateText)
    {
        var update = FluentTwitter.CreateRequest(info)
                     .AuthenticateAs(username, password)
                     .Statuses()
                     .Update(updateText).AsJson();

        return update.Request();
    }
}

