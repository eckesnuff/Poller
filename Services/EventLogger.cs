using System;
using System.Diagnostics;

namespace Poller.Services {
    class EventLogger : ILogger {
        const string Source = "Poller";
        const string LogName = "Application";
        public void Log(string message, Domain.LogType logType) {
            if (!EventLog.SourceExists(Source)) {
                EventLog.CreateEventSource(Source, LogName);
            }
            EventLog.WriteEntry(Source, message,
                                TranslateEntryType(logType));
        }
        protected EventLogEntryType TranslateEntryType(Domain.LogType logType) {
            switch (logType) {
                case Domain.LogType.Error:
                    return EventLogEntryType.Error;
                case Domain.LogType.Warning:
                    return EventLogEntryType.Warning;
                case Domain.LogType.Info:
                    return EventLogEntryType.Information;
                default:
                    throw new NotImplementedException(string.Format("LogType: {0} has no mapped type", logType));

            }
        }
    }
}