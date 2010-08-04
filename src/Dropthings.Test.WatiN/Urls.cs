using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.Test.WatiN
{
    public class Urls
    {
        public const string Host = "http://localhost:8000/Dropthings";

        public const string Homepage = Host + "/Default.aspx";
        public const string LoginPage = Host + "/LoginPage.aspx";
        public const string LogoutPage = Host + "/Logout.ashx";
        public const string StartOverPage = Host + "/Logout.ashx";

    }
}
