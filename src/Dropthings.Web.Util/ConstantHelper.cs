using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Dropthings.Web.Util
{
    public class ConstantHelper
    {
        public static readonly string ScriptVersionNo = ConfigurationManager.AppSettings["ScriptVersionNo"] ?? string.Empty;
    }
}
