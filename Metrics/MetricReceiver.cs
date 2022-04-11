using System.Diagnostics;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Commons.Receiver;
using System;
using System.Collections.Generic;
using Commons.Entities;

namespace Metrics
{
    public class MetricReceiver : Receiver<IMetric>, IMetricReceiver
    {
        private static MetricReceiver _instance;

        public static MetricReceiver GetInstance()
        {
            if (_instance == null)
            {
                Initialize(
                    new MetricReceiverConfig(),
                    new List<IPublisher<IMetric>> {
                            PublisherFactory.Create<IMetric>(PublisherTypes.File, FilePublisher<IMetric>.CreateDefaultConfig("metrics"), new Formatters.JsonFormatter())
                        }
                    );
            }

            return _instance;
        }

        public static void Initialize(IReceiverConfig config, IEnumerable<IPublisher<IMetric>> publishers)
        {
            _instance = new MetricReceiver(config, publishers);            
        }

        private MetricReceiver(IReceiverConfig config, IEnumerable<IPublisher<IMetric>> publishers)
        {
            _config = config;
            _publishers = publishers;

            Start();
        }
    }
}
