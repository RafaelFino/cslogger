using Commons.Entities;
using Commons.Interfaces;
using System.Text.Json;

namespace Metrics.Formatters
{
    public class JsonFormatter : IFormatter<Metric>
    {
        public string Format(Metric data)
        {
            if (data != null)
            {
                return JsonSerializer.Serialize<Metric>(data, new JsonSerializerOptions() { IgnoreNullValues = true });
            }

            return string.Empty;
        }
    }
}
