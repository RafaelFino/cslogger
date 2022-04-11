using System.Diagnostics;
using Commons.Enum;
using Commons.Interfaces;
using Commons.Publishers;
using Commons.Receiver;
using System;
using System.Collections.Generic;
using Commons.Entities;

namespace Commons.Interfaces
{
    public interface IMetricReceiver : IReceiver<IMetric>, IDisposable
    {
        
    }
}