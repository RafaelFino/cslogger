using Commons.Interfaces;
using System.Text.Json;

namespace Logger.Formatters
{
    public class JsonFormatter : IFormatter<ILogMessage>
    {
        public string Format(ILogMessage message)
        {
            if (message != null)
            {
                return JsonSerializer.Serialize<ILogMessage>(message, new JsonSerializerOptions() { IgnoreNullValues = true });
            }

            return string.Empty;
        }
    }
}
