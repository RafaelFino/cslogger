using System.Text.Json.Serialization;

namespace Commons.Enum
{
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum MetricType
    {
        Gauge = 0,
        Counter = 1,
        Duration = 2
    }
}