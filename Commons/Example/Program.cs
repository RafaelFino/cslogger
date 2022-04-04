using System;
using System.Collections.Generic;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Commons.Entities;
using Logger;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var loggerConfig = new LoggerConfig();

            loggerConfig.Level = LogLevel.Debug;

            MessageLogger.Initialize(loggerConfig, new List<IPublisher<ILogMessage>>
                {
                    PublisherFactory.Create(
                        PublisherTypes.Console,
                        ConsolePublisher<ILogMessage>.CreateDefaultConfig(),
                        new Logger.Formatters.TextFormatter()),

                    PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<ILogMessage>.CreateDefaultConfig(),
                        new Logger.Formatters.JsonFormatter())
                }
            );


            using (var logger = MessageLogger.GetInstance())
            {
                logger.Info("Teste - Info");

                for (int i = 0; i < 100; i++)
                {
                    logger.Debug($"Teste - debug {i}");
                }

                logger.Info("Fim! - Info");
            }
        }
    }
}
