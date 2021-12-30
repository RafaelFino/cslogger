using System;
using System.Collections.Generic;

namespace CSLoggerLib
{
    public class Entry
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Messsage { get; set; }
        public string Source { get; set; }  
        public LogLevel Level { get; set; } 
        public IDictionary<string, object> Details { get; set; }
    }
}

