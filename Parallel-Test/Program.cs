using Commons.Entities;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Logger;
using Metrics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Initialize();

                MessageLogger.GetInstance().Debug("App starting");
                using (var m = new MetricContext("global-app"))
                {
                    StartApp();
                }
                MessageLogger.GetInstance().Debug("App stop");
            }
            catch (Exception ex)
            {
                MessageLogger.GetInstance().Fatal("main::fatal", ex);
                throw;
            }
            finally
            {
                MessageLogger.GetInstance().Stop();
                MetricReceiver.GetInstance().Stop();
            }
        }

        static void Initialize()
        {
            //Logger
            MessageLogger.Initialize(new LoggerConfig()
            {
                Level = LogLevel.Debug
            },
                new List<IPublisher<ILogMessage>>
                {
                    PublisherFactory.Create(
                        PublisherTypes.Console,
                        ConsolePublisher<ILogMessage>.CreateDefaultConfig("parallel-test"),
                        new Logger.Formatters.TextFormatter()),

                    PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<ILogMessage>.CreateDefaultConfig("parallel-log"),
                        new Logger.Formatters.JsonFormatter())
                }
            );

            //Metrics
            MetricReceiver.Initialize(new MetricReceiverConfig(),
                new List<IPublisher<IMetric>>()
                {
                    PublisherFactory.Create(
                        PublisherTypes.File,
                        FilePublisher<IMetric>.CreateDefaultConfig("parallel-metric"),
                        new Metrics.Formatters.JsonFormatter())
                });

            Logger = MessageLogger.GetInstance();
        }

        static ILogger Logger;

        static void StartApp()
        {
            var sizes = new List<int>
            {
                10 //warm up
                ,100
                ,100
                ,100
                ,100
                ,int.MaxValue / 50000
                ,int.MaxValue / 5000
                ,int.MaxValue / 1000
            };

            var runners = new Dictionary<string, Func<int, Func<string>, long>>();
            runners.Add("Sequencial", SequencialFor);
            runners.Add("Bad Parallel", BadParallelFor);
            runners.Add("Concurrent Parallel", ParallelForConcurrentBag);

            var formatters = new Dictionary<string, Func<string>>();
            formatters.Add("FastString", GetFastString);
            formatters.Add("LongString", GetLongString);
            formatters.Add("ForceCPU", ForceCPUString);

            var results = new List<string>();
            results.Add($"| {"Processor".PadRight(20)}| {"Formatter".PadRight(20)}| ElapsedTime |");

            foreach (var r in runners.Keys)
            {
                foreach (var f in formatters.Keys)
                {
                    Logger.Info($"{r}:{f}");
                    foreach (var s in sizes)
                    {
                        var elapsed = runners[r](s, formatters[f]);
                        results.Add($"| {r.PadRight(20)}| {f.PadRight(20)}| {elapsed:00000000000} |");
                    }
                }
            }

            //Make a TABLE
            var line = "".PadRight(results[results.Count - 1].Length, '-');

            results.Insert(0, line);
            results.Insert(2, line);
            results.Add(line);

            foreach (var l in results)
            {
                Console.WriteLine(l);
            }
        }

        //try to be simple
        public static string GetFastString()
        {
            return Guid.NewGuid().ToString();
        }

        //eat a lot of RAM
        public static string GetLongString()
        {
            return Guid.NewGuid().ToString().PadRight(1000).PadRight(9000, '@').PadLeft(1000, '#');
        }

        //use CPU like there's no tomorrow - NEVER TRY SOMETHING LIKE THAT!
        public static string ForceCPUString()
        {
            using (SHA256 code = SHA256.Create())
            {
                var g = Guid.NewGuid().ToString().ToUpperInvariant();
                var arr = String.Empty;

                for (int j = 0; j < 100; j++)
                {
                    g += j % 2 == 0 ?
                        Guid.NewGuid().ToString().ToLowerInvariant()
                        : Guid.NewGuid().ToString().ToUpperInvariant();                              
                }

                //                   \o/         
                foreach (var item in g.Replace('a', 'A').Replace('A', 'a').ToCharArray())
                {
                    //Guid never will have this char
                    if (!g.Contains('#'))
                    {
                        arr += item.ToString();
                    }
                }

                var barr = Encoding.ASCII.GetBytes(arr);
                var hash = code.ComputeHash(barr);
                var str = System.Text.Encoding.UTF8.GetString(hash);

                return str.ToUpperInvariant();
            }
        }

        //simple way
        public static long SequencialFor(int size, Func<string> formatter)
        {
            var sw = Stopwatch.StartNew();
            var data = new List<string>(size);
            using (var metric = new MetricContext("SequencialFor"))
            {
                for (int i = 0; i < size; i++)
                {
                    data.Add(formatter());
                }
                metric.Add("size", size);
            }

            sw.Stop();
            Logger.Info($"[{sw.ElapsedMilliseconds:0000000}ms] SequencialFor :: {size}/{data.Count} -> result: {size == data.Count}");
            data.Clear();
            return sw.ElapsedMilliseconds;
        }

        //never do that
        public static long BadParallelFor(int size, Func<string> formatter)
        {
            var sw = Stopwatch.StartNew();
            var data = new List<string>(size);

            using (var metric = new MetricContext("BadParallelFor"))
            {
                Parallel.For(0, size, i =>
                {
                    data.Add(formatter());
                });
                metric.Add("size", size);
            }

            sw.Stop();
            Logger.Info($"[{sw.ElapsedMilliseconds:0000000}ms] BadParallelFor :: {size}/{data.Count} -> result: {size == data.Count}");
            data.Clear();
            return sw.ElapsedMilliseconds;
        }

        //best way to big data
        public static long ParallelForConcurrentBag(int size, Func<string> formatter)
        {
            var sw = Stopwatch.StartNew();
            var data = new ConcurrentBag<string>();

            using (var metric = new MetricContext("ParallelForConcurrentBag"))
            {
                Parallel.For(0, size, i =>
                {
                    data.Add(formatter());
                });
                metric.Add("size", size);
            }

            sw.Stop();
            Logger.Info($"[{sw.ElapsedMilliseconds:0000000}ms] ParallelForConcurrentBag :: {size}/{data.Count} -> result: {size == data.Count}");
            data.Clear();
            return sw.ElapsedMilliseconds;
        }
    }
}
