using System.Data.Common;
using Commons.Entities;
using Commons.Interfaces;

namespace Metrics.Formatters
{
    public class TextFormatter : IFormatter<IMetric>
    {
        string IFormatter<IMetric>.Format(IMetric data)
        {
            if(data != null)
            {
                return string.Format($"{data.Namespace};{FormatTime(data)};{data.Value};{data.Type};{FormatTags(data)}");
            }

            return string.Empty;
        }         

        static string FormatTime(IMetric input)
        {
            return input.Timespam.ToString("yyyyMMdd HHmmss.fffffff K");
        }

        static string FormatTags(IMetric input)
        {
            if (input.Tags != null)
            {
                return string.Join(",", input.Tags) + ";";
            }

            return string.Empty;
        }          
    }
}