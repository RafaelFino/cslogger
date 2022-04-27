using Commons.Entities;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Logger;
using Metrics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitializeLog();
            InitializeMetricReceiver();

            using (var metricReceiver = MetricReceiver.GetInstance())
            {
                using (var m = new MetricContext("global-process"))
                {
                    using (var logger = MessageLogger.GetInstance())
                    {
                        RunTest();
                    }
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
            decimal qtd = 500;
            decimal count = 0;

            using (var metrics = new MetricContext("example.test"))
            {
                var timer = new Stopwatch();
                timer.Start();

                logger.Info("Test Start - Info");

                var asyncTaskDebug = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < qtd; i++)
                    {
                        logger.Debug($"Async with sleep - debug {i}");
                        Thread.Sleep(1);
                    }
                });

                var asyncTaskInfo = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < qtd; i++)
                    {
                        logger.Info($"Async with sleep - info {i}");
                        Thread.Sleep(1);
                    }
                });

                while (!asyncTaskDebug.IsCompleted || !asyncTaskInfo.IsCompleted)
                {
                    for (int i = 0; i < qtd / 10; i++)
                    {
                        logger.Debug($"Sync - debug {i}/{count}");
                        count++;
                    }
                    Thread.Sleep(1);
                }

                asyncTaskDebug.Wait();
                asyncTaskInfo.Wait();

                timer.Stop();

                metrics.Add("average-time", Convert.ToDecimal(timer.Elapsed.TotalMilliseconds) / (count + qtd * 2), MetricType.Duration);
                metrics.Add("tps", (count + qtd * 2) / (Convert.ToDecimal(timer.Elapsed.TotalMilliseconds * 1000)), MetricType.Gauge);

                metrics.Add("processes-sync", count);
                metrics.Add("processes-async", qtd * 2);
                metrics.Add("processes-total", count + qtd * 2);

                logger.Info("Test Finish - Info");
            }
        }
    }
}