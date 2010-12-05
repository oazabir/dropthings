using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Dropthings.DiggSilverlight
{
    public class DynamicEndpointHelper
    {
        // Put the development server site URL including the trailing slash
        // This should be same as what's set in the Dropthings web project's 
        // properties as the URL of the site in development server
        private const string BaseUrl = "http://localhost:8000/Dropthings/";

        public static string ResolveEndpointUrl(string endpointUrl, string xapPath)
        {
            string baseUrl = xapPath.Substring(0, xapPath.IndexOf("ClientBin"));
            string relativeEndpointUrl = endpointUrl.Substring(BaseUrl.Length);
            string dynamicEndpointUrl = baseUrl + relativeEndpointUrl;
            return dynamicEndpointUrl;
        }
    }
}
