using Commons.Entities;
using Commons.Interfaces;
using System.Text.Json;

namespace Metrics.Formatters
{
    public class JsonFormatter : IFormatter<IMetric>
    {
        string IFormatter<IMetric>.Format(IMetric data)
        {
            if (data != null)
            {
                return JsonSerializer.Serialize<IMetric>(data, new JsonSerializerOptions() { IgnoreNullValues = true });
            }

            return string.Empty;
        }
    }
}
