using Commons.Interfaces;
using System;
using System.Collections.Generic;
using System;

namespace Commons.Publishers
{
    public class ConsolePublisher<T> : IPublisher<T>
    {
        private readonly IDictionary<string, object> _config;
        private readonly IFormatter<T> _formatter;

        private string _name;

        public const string TagName = "name";

        public static IDictionary<string, object> CreateDefaultConfig(string name)
        {
            Console.WriteLine($"[{name}] Creating default config");
            return new Dictionary<string, object>()
            {
                { TagName, name } 
            };
        }

        public ConsolePublisher(IDictionary<string, object> config, IFormatter<T> formatter)
        {                        
            _config = config;
            _formatter = formatter;
            _name = (string)config[TagName];
        }

        public void Publish(T message)
        {
            Console.WriteLine($"[{_name}] {_formatter.Format(message)}");
        }

        public void Publish(IEnumerable<T> messages)
        {
            foreach (var message in messages)
            {
                Console.WriteLine($"[{_name}] {_formatter.Format(message)}");
            }            
        }        

        public void Start()
        {
            Console.WriteLine($"[{_name}] Start console publisher");
        }

        public void Stop()
        {
            Console.WriteLine($"[{_name}] Stop console publisher");
        }
    }
}
