using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Dropthings.Util
{
    public class EntLibLogger : ILogger
    {

        #region ILogger Members

        public void Log(string message)
        {
            if (Logger.IsLoggingEnabled())
            {
                var newLogEntry = new LogEntry
                {
                    Message = message,
                    Severity = TraceEventType.Information
                };
                
                if (Logger.ShouldLog(newLogEntry))
                    Logger.Write(newLogEntry);
            }
        }

        public void Log(string[] categories, string message)
        {
            if (Logger.IsLoggingEnabled())
            {
                var newLogEntry = new LogEntry
                {
                    Message = message,
                    Severity = TraceEventType.Information
                };
                categories.Each((category) => newLogEntry.Categories.Add(category));

                if (Logger.ShouldLog(newLogEntry))
                    Logger.Write(newLogEntry);
            }
        }

        #endregion

        #region ILogger Members


        public void LogException(Exception x)
        {
            bool rethrow = ExceptionPolicy.HandleException(x, "Log Only");
            if (rethrow)
                throw x;
        }

        #endregion
    }
}
