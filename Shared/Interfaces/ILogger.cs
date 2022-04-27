using System;

namespace Commons.Interfaces
{
    public interface ILogger : IReceiver<ILogMessage>
    {
        void Debug(string message);
        void Debug(ILogMessage message);

        void Info(string message);
        void Info(ILogMessage message);

        void Warn(string message);
        void Warn(ILogMessage message);

        void Error(string message);

        void Error(string message, Exception ex);
        void Error(ILogMessage message);

        void Fatal(string message);

        void Fatal(string message, Exception ex);
        void Fatal(ILogMessage message);
    }
}
