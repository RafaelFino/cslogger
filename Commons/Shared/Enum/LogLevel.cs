using System.Text.Json.Serialization;

namespace Commons.Enum
{
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum LogLevel
    {
        Fatal = 0,
        Error = 1,
        Warn = 2,    
        Info = 3,
        Debug = 4           
    }
}
