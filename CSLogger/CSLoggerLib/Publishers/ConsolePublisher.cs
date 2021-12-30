using System;
using System.Collections.Generic;
using System.Text;

namespace CSLoggerLib
{
    public class ConsolePublisher : IPublisher
    {
        public IEntryFormatter Formatter { get; set; }
        public IList<string> ConfigTags { get; set; } = null;

        public void Publish(Entry entry)
        {
            string line = Formatter.Format(entry);
            Console.WriteLine(line);
        }

        public void Start(IDictionary<string, object> config = null)
        {
            return;
        }

        public void Stop()
        {
            return;
        }
    }
}
