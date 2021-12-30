using System.Collections.Generic;

namespace CSLoggerLib
{
    public interface IPublisher
    {
        IEntryFormatter Formatter { set;  get; }
        IList<string> ConfigTags { get; set; }

        void Publish(Entry entry);

        void Start(IDictionary<string, object> config = null);
        void Stop();
    }
}

