using System;
using System.Collections.Generic;
using Commons.Enum;

namespace Commons.Interfaces
{
    public interface IMetric
    {
        public string Namespace { get; set; }
        public decimal Value { get; set; }  
        public DateTime Timespam { get; set; }  
        public IEnumerable<string> Tags { get; set; }
        public MetricType Type { get; set; }
    }
}