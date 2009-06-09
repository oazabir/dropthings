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
            var scriptLoader = new StringBuilder();

            scriptLoader.Append("ensure( { js: '");
            scriptLoader.Append(string.Format("{0}", scriptToLoad)); 
            scriptLoader.Append("' }, function() { ");
            scriptLoader.Append(string.Format("{0}", startUpCode)); 
            scriptLoader.Append(" } );");

            ScriptManager.RegisterStartupScript(controlToBind, controlToBind.GetType(), controlToBind.ToString() + DateTime.Now.Ticks.ToString(), 
                scriptLoader.ToString(), true);
        }
    }
}
