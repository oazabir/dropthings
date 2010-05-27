using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

namespace Dropthings.Util
{
    public class JsonHelper
    {
        public static string Serialize<T>(T item)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializer.MissingMemberHandling = MissingMemberHandling.Error;
            jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            
            StringBuilder buffer = new StringBuilder();
            using (StringWriter sw = new StringWriter(buffer))
            {
                using (JsonTextWriter jtw = new JsonTextWriter(sw))
                {
                    jsonSerializer.Serialize(jtw, item);
                }
            }

            return buffer.ToString();
        }
    }
}
