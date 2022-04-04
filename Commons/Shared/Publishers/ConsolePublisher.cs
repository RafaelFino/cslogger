using Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Publishers
{
    public class ConsolePublisher<T> : IPublisher<T>
    {
        private readonly IDictionary<string, object> _config;
        private readonly IFormatter<T> _formatter;
        public static IDictionary<string, object> CreateDefaultConfig()
        {
            return new Dictionary<string, object>();
        }

        public ConsolePublisher(IDictionary<string, object> config, IFormatter<T> formatter)
        {
            _config = config;
            _formatter = formatter;
        }

        public void Publish(T message)
        {
            Console.WriteLine(_formatter.Format(message));
        }

        public void Start()
        {
            Console.WriteLine("###  Starting console publisher                                                             ###");
        }

        public void Stop()
        {
            Console.WriteLine("###  Stopping console publisher                                                             ###");
        }
    }
}
