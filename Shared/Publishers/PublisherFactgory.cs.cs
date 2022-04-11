using Commons.Interfaces;
using Commons.Enum;
using System.Collections.Generic;

namespace Commons.Publishers
{
    public class PublisherFactory
    {
        public static IPublisher<T> Create<T>(PublisherTypes t, IDictionary<string, object> config, IFormatter<T> formatter)
        {
            IPublisher<T> ret = null;

            switch (t)
            {                  
                case PublisherTypes.File:
                    ret = new FilePublisher<T>(config, formatter);
                    break;
                
                case PublisherTypes.Console:
                default:
                    ret = new ConsolePublisher<T>(config, formatter);
                    break;
            }

            return ret;
        }
    }
}
