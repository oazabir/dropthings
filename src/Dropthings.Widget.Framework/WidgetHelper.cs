using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI;
using System.Web;
using System.IO;

namespace Dropthings.Widget.Framework
{
    public class WidgetHelper
    {
        public static void RegisterWidgetScript(Control controlToBind, string scriptToLoad, string startUpCode)
        {
            RegisterWidgetScript(controlToBind, new string[] { scriptToLoad }, null, null, startUpCode);
        }
        public static void RegisterWidgetScript(Control controlToBind, string[] scriptsToLoad, string startUpCode)
        {
            RegisterWidgetScript(controlToBind, scriptsToLoad, null, null, startUpCode);
        }
        public static void RegisterWidgetScript(Control controlToBind, 
            string[] scriptsToLoad, 
            string[] cssToLoad, 
            string[] htmlSnippetsToLoad,
            string startUpCode)
        {
        
            var scriptLoader = new StringBuilder();

            scriptLoader.Append("ensure( { ");
            
            if (scriptsToLoad != null)
            {
                scriptLoader.Append("js: ['");
                scriptLoader.Append(string.Join("','", scriptsToLoad));
                scriptLoader.Append("'],");
            }
            if (cssToLoad != null)
            {
                scriptLoader.Append("css: ['");
                scriptLoader.Append(string.Join("','", cssToLoad));
                scriptLoader.Append("'],");
            }
            if (htmlSnippetsToLoad != null)
            {
                scriptLoader.Append("html: ['");
                scriptLoader.Append(string.Join("','", htmlSnippetsToLoad));
                scriptLoader.Append("'],");
            }

            if (scriptLoader[scriptLoader.Length - 1] == ',')
                scriptLoader.Length--;

            scriptLoader.Append("}, function() { ");
            scriptLoader.Append(string.Format("{0}", startUpCode)); 
            scriptLoader.Append(" } );");

            ScriptManager.RegisterStartupScript(controlToBind, controlToBind.GetType(), controlToBind.ToString() + DateTime.Now.Ticks.ToString(), 
                scriptLoader.ToString(), true);
        }
    }
}
