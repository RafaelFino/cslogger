using System;
using System.Collections.Generic;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Commons.Entities;
using Logger;
using Metrics;

namespace Example
{    
    internal class Program
    {
        static void Main(string[] args)
        {                                    
            InitializeLog();
            InitializeMetricReceiver();

            using(var metricReceiver = MetricReceiver.GetInstance())
            {
                using(var logger = MessageLogger.GetInstance())
                {
                    RunTest();
                }
            }
        }

        static void InitializeLog()
        {

            MessageLogger.Initialize(new LoggerConfig() 
                { 
                    Level = LogLevel.Debug 
                }, 
                new List<IPublisher<ILogMessage>>
                {
                    PublisherFactory.Create(
                        PublisherTypes.Console,
                        ConsolePublisher<ILogMessage>.CreateDefaultConfig("log-text"),
                        new Logger.Formatters.TextFormatter()),

                    PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<ILogMessage>.CreateDefaultConfig("log-json"),
                        new Logger.Formatters.JsonFormatter())
                }
            );
        }

        static void InitializeMetricReceiver()
        {
            var metricConfig = new MetricReceiverConfig();
            var textPublisher = PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<IMetric>.CreateDefaultConfig("metric-text"),
                        new Metrics.Formatters.TextFormatter());

            var jsonPublisher = PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<IMetric>.CreateDefaultConfig("metric-json"),
                        new Metrics.Formatters.JsonFormatter());

            var publishers = new List<IPublisher<IMetric>>(); 
            publishers.Add(textPublisher);
            publishers.Add(jsonPublisher);            
            
            MetricReceiver.Initialize(metricConfig, publishers); 
        }        

        static void RunTest()
        {
            var logger = MessageLogger.GetInstance();
            int qtd = 5000;
            
            using(var metrics = new MetricContext("Text"))
            {
                logger.Info("Teste - Info");

                for (int i = 0; i < qtd; i++)
                {
                    logger.Debug($"Teste - debug {i}");
                }

                metrics.Add("processes", qtd);

                logger.Info("Fim! - Info");            
            }
        }
    }
}
