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
    public class MetricContext : IDisposable
    {
        private readonly string _namespace;
        private DateTime _start = DateTime.Now;

        private Stopwatch _clock = new Stopwatch();
        private List<IMetric> _data = new List<IMetric>();

        private IEnumerable<string> _tags;

        public MetricContext(string contextNamespace, IEnumerable<string> tags = null)
        {
            _clock.Start();
            _namespace = contextNamespace;
            _tags = tags;
        }        

        public void Add(string name, decimal value) 
        {
            _data.Add(new Metric() { Namespace = string.Format($"{_namespace}.{name}"), Value = value, Timespam = DateTime.Now, Type = MetricType.Counter, Tags = _tags });
        }

        public void Add(string name, decimal value, MetricType metricType) 
        {
            _data.Add(new Metric() { Namespace = string.Format($"{_namespace}.{name}"), Value = value, Timespam = DateTime.Now, Type = metricType, Tags = _tags });
        }        
        public void Add(IMetric metric)
        {
            _data.Add(metric);
        }

        public void Add(IEnumerable<IMetric> metrics)
        {
            _data.AddRange(metrics);
        }

        public void Dispose()
        {
            MetricReceiver.GetInstance().Send(_data);
            _clock.Stop();
            MetricReceiver.GetInstance().Send(new Metric() {
                Namespace = string.Format($"{_namespace}.context-elapsedtime"),
                Timespam = _start,
                Value = Convert.ToDecimal(_clock.Elapsed.TotalMilliseconds),
                Type = MetricType.Duration,
                Tags = _tags
            });
        }
    } 
}