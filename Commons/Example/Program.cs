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
            /*
            var loggerConfig = new LoggerConfig();

            loggerConfig.Level = LogLevel.Debug;
            //loggerConfig.Level = LogLevel.Info;

            var textPublisherConfig = FilePublisher<ILogMessage>.CreateDefaultConfig();
            textPublisherConfig[FilePublisher<ILogMessage>.TagExtensionFilename] = "log";

            var jsonPublisherConfig = FilePublisher<ILogMessage>.CreateDefaultConfig();                        
            jsonPublisherConfig[FilePublisher<ILogMessage>.TagExtensionFilename] = "json";

            MessageLogger.Initialize(loggerConfig, new List<IPublisher<ILogMessage>>
                {
                    PublisherFactory.Create(PublisherTypes.File, textPublisherConfig, new Logger.Formatters.TextFormatter()),
                    PublisherFactory.Create(PublisherTypes.File, jsonPublisherConfig, new Logger.Formatters.JsonFormatter())
                }
            );
            */

            using (var logger = MessageLogger.GetInstance())
            { 
                logger.Info("Teste - Info");

                for (int i = 0; i < 100; i++)
                {
                    logger.Debug($"Teste - debug {i}");
                    Console.WriteLine($"Linha {i}");
                }

                logger.Info("Fim! - Info");

                Console.WriteLine($"Fim!");
            }
        }
    }
}
