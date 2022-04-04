namespace Commons.Interfaces
{
    public interface ILogger
    {
        void Debug(string message);
        void Debug(ILogMessage message);

        void Info(string message);
        void Info(ILogMessage message);

        void Warn(string message);
        void Warn(ILogMessage message);

        void Error(string message);
        void Error(ILogMessage message);

        void Fatal(string message);
        void Fatal(ILogMessage message);
    }
}
