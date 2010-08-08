using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using System.Diagnostics;
using System.Threading;

namespace Dropthings.Test.WatiN
{
    internal static class BrowserHelper
    {
        internal static Browser OpenNewBrowser(string url)
        {
            Browser browser = new IE();
            browser.ShowWindow(global::WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowNormal);
            browser.GoTo(url);
            browser.WaitForComplete();
            return browser;
        }

        internal static int[] FindPosition(this Browser browser, Element e)
        {
            var top = 0;
            var left = 0;

            var item = e;
            while (item != null)
            {
                top += int.Parse(item.GetAttributeValue("offsetTop"));
                left += int.Parse(item.GetAttributeValue("offsetLeft"));

                item = item.Parent;
            }

            return new int [] { left, top };
        }

        internal static bool WaitForAsyncPostbackComplete(this Browser browser, int timeout)
        {
            int timeWaitedInMilliseconds = 0;
            var maxWaitTimeInMilliseconds = Settings.WaitForCompleteTimeOut * 1000;
            var scriptToCheck = "Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();";

            while (bool.Parse(browser.Eval(scriptToCheck)) == true
                    && timeWaitedInMilliseconds < maxWaitTimeInMilliseconds)
            {
                Thread.Sleep(Settings.SleepTime);
                timeWaitedInMilliseconds += Settings.SleepTime;
            }

            return bool.Parse(browser.Eval(scriptToCheck));
        }

        internal static void WaitForJQueryAjaxRequest(this Browser browser)
        {
            int timeWaitedInMilliseconds = 0;
            var maxWaitTimeInMilliseconds = Settings.WaitForCompleteTimeOut * 1000;

            while (browser.IsJQueryAjaxRequestInProgress()
                    && timeWaitedInMilliseconds < maxWaitTimeInMilliseconds)
            {
                Thread.Sleep(Settings.SleepTime);
                timeWaitedInMilliseconds += Settings.SleepTime;
            }
        }

        internal static bool IsJQueryAjaxRequestInProgress(this Browser browser)
        {
            var evalResult = browser.Eval("watinAjaxMonitor.isRequestInProgress()");
            return evalResult == "true";
        }

        internal static void InjectJQueryAjaxMonitor(this Browser browser)
        {
            const string monitorScript =
                @"function AjaxMonitor(){"
                + "var ajaxRequestCount = 0;"

                + "$(document).ajaxStart(function(){"
                + "    ajaxRequestCount++;"
                + "});"

                + "$(document).ajaxComplete(function(){"
                + "    ajaxRequestCount--;"
                + "});"

                + "this.isRequestInProgress = function(){"
                + "    return (ajaxRequestCount > 0);"
                + "};"
                + "}"

                + "var watinAjaxMonitor = new AjaxMonitor();";

            browser.Eval(monitorScript);
        }        

        private static void ClearCookiesInIE()
        {
            Process.Start("RunDll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2").WaitForExit();
        }

        private static void DeleteEverythingInIE()
        {
            Process.Start("RunDll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255").WaitForExit();            
        }

        internal static void ClearCookies()
        {
            ClearCookiesInIE();
        }
    }
}
