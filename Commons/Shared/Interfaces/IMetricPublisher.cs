using Commons.Entities;
using System.Collections.Generic;

namespace Commons.Interfaces
{
    public interface IMetricPublisher
    {
        public void Start();

        public void Stop();

        public void Publish(IEnumerable<Metric> metrics);
    }
}
