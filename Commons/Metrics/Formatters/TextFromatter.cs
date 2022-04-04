using Commons.Entities;
using Commons.Interfaces;

namespace Metrics.Formatters
{
    public class TextFromatter : IFormatter<Metric>
    {
        string IFormatter<Metric>.Format(Metric data)
        {
            if(data != null)
            {
                return string.Format($"{data.Namespace};{data.Timespam};{data.Value};{data.Type};{string.Join(",", data.Tags)};");
            }

            return string.Empty;
        }
    }
}