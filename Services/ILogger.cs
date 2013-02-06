using Poller.Domain;

namespace Poller.Services {
    public interface ILogger {
        void Log(string message, LogType logType);
    }
}
