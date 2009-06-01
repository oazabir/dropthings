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
        public static void RegisterWidgetScript(Control controlToBind, string pathToClientScript) 
        {
            using (var reader = new StreamReader(HttpContext.Current.Server.MapPath(pathToClientScript)))
            {
                var content = reader.ReadToEnd();
                reader.Close();
                ScriptManager.RegisterStartupScript(controlToBind, controlToBind.GetType(), controlToBind.ToString() + DateTime.Now.Ticks.ToString(), content, true);
            }
        }
    }
}
