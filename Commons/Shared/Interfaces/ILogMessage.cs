using Commons.Enum;
using System;
using System.Collections.Generic;

namespace Commons.Interfaces
{
    public interface ILogMessage
    {
        public DateTime When { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public IDictionary<string, object> Details { get; }
        public Exception Ex { get; set; }
    }
}
