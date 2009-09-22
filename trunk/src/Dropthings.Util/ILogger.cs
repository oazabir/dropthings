using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dropthings.Util
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string[] categories, string message);
        void LogException(Exception x);
    }
}
