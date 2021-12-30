using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSLoggerLib
{
    public class TextFormatter : IEntryFormatter
    {
        public string Format(Entry entry)
        {
            return String.Format("{0} {1} {2} {3} {4}", 
                entry.DateTime, 
                entry.Level, 
                entry.Source, 
                entry.Messsage, 
                entry.Details != null 
                    ? string.Join("\t", entry.Details) 
                    : string.Empty
                );
        }
    }
}
