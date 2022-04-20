
using Commons.Enum;
using Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Entities
{
    public class LogMessage : ILogMessage
    {
        public DateTime When { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public Exception Ex { get; set; }

        public IDictionary<string, object> Details { get; set; }

        public LogMessage(LogLevel level, string message) 
        {
            this.When = DateTime.Now;
            this.Message = message;
            this.Level = level;
        }

        public LogMessage(LogLevel level, string message, IDictionary<string, Object> details)
        {
            this.When = DateTime.Now;
            this.Message = message;
            this.Level = level;
            this.Details = details;
        }

        public override string ToString()
        {
            string ret;
            string level = Level.ToString();

            if (Details != null && Details.Count > 0)
            {
                ret = string.Format($"{When}\t{level}\t{Message}\t{string.Join("\t", Details.Select((k, v) => string.Format($"{k}:{v} ")))}");
            } 
            else
            {
                ret = string.Format($"{When}\t{level}\t{Message}");
            }

            if( Ex != null )
            {
                ret += $"\tException: {Ex}";
            }

            return ret;
        }
    }
}
