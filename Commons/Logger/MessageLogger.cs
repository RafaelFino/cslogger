using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Commons.Receiver;
using System;
using System.Collections.Generic;
using Commons.Entities;

namespace Logger
{
    public class MessageLogger : Receiver<ILogMessage>, ILogger, IDisposable
    {
        private static MessageLogger _instance;

        private readonly ISet<LogLevel> _levels = new HashSet<LogLevel>();
        private readonly LoggerConfig _logConfig;

        public static MessageLogger GetInstance()
        {
            if (_instance == null)
            {
                Initialize(
                    new LoggerConfig() { 
                        Level = LogLevel.Debug 
                    },
                    new List<IPublisher<ILogMessage>> {
                            PublisherFactory.Create<ILogMessage>(PublisherTypes.File, FilePublisher<ILogMessage>.CreateDefaultConfig(), new Formatters.JsonFormatter())
                        }
                    );
            }

            return _instance;
        }
        public static void Initialize(IReceiverConfig config, IEnumerable<IPublisher<ILogMessage>> publishers)
        {
            _instance = new MessageLogger(config, publishers);            
        }

        private MessageLogger(IReceiverConfig config, IEnumerable<IPublisher<ILogMessage>> publishers)
        {
            _config = config;
            _publishers = publishers;

            if (config is LoggerConfig)
            {
                _logConfig = config as LoggerConfig;
            }
            else
            {
                _logConfig = new LoggerConfig() { Level = LogLevel.Debug };
            }

            foreach (var lvl in (LogLevel[])System.Enum.GetValues(typeof(LogLevel)))
            {
                if (lvl <= _logConfig.Level)
                {
                    _levels.Add(lvl);
                }
            }

            Start();
        }

        #region Send message methods
        public void Debug(ILogMessage message)
        {
            message.Level = LogLevel.Debug;
            Publish(message);
        }

        public void Debug(string message)
        {
            Publish(new LogMessage(LogLevel.Debug, message));
        }

        public void Error(ILogMessage message)
        {
            message.Level = LogLevel.Error;
            Publish(message);
        }

        public void Error(string message)
        {
            Publish(new LogMessage(LogLevel.Error, message));
        }

        public void Fatal(ILogMessage message)
        {
            message.Level = LogLevel.Fatal;
            Publish(message);
        }

        public void Fatal(string message)
        {
            Publish(new LogMessage(LogLevel.Fatal, message));
        }

        public void Info(ILogMessage message)
        {
            message.Level = LogLevel.Info;
            Publish(message);
        }

        public void Info(string message)
        {
            Publish(new LogMessage(LogLevel.Info, message));
        }

        public void Warn(ILogMessage message)
        {
            message.Level = LogLevel.Warn;
            Publish(message);
        }

        public void Warn(string message)
        {
            Publish(new LogMessage(LogLevel.Warn, message));
        }
        #endregion
    }
}
