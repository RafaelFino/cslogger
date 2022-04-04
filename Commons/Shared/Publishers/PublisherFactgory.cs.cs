using Commons.Interfaces;
using Commons.Enum;
using System.Collections.Generic;

namespace Commons.Publishers
{
    public class PublisherFactory
    {
        public static IPublisher<T> Create<T>(PublisherTypes t, IDictionary<string, object> config, IFormatter<T> formatter)
        {
            switch(t)
            {
                case PublisherTypes.File:
                default:
                    return new FilePublisher<T>(config, formatter);
            }
        }
    }
}
