﻿using Commons.Enum;
using System;
using System.Collections.Generic;

namespace Commons.Entities
{
    public class Metric
    {
        public string Namespace { get; set; }
        public decimal Value { get; set; }  
        public DateTime Timespam { get; set; }  
        public IEnumerable<string> Tags { get; set; }
        public MetricType Type { get; set; }
    }
}
