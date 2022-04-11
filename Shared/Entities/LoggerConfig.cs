using Commons.Enum;
using Commons.Interfaces;

namespace Commons.Entities
{
    public class LoggerConfig : ReceiverConfig
    {        
        public LogLevel Level = LogLevel.Debug;
    }
}
