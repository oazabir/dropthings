using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Dropthings.Web.Util
{
    public static class ExtensionMethods
    {        
        public static string ToJson(this object o, int recursionLimit)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionLimit;
            return serializer.Serialize(o);
        }
        public static string ToJson(this object o)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(o);
        }
    }
}
