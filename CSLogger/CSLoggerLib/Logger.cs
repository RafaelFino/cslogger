using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSLoggerLib
{
    public class Logger: IDisposable
    {
        private ConcurrentQueue<Entry> _entries = new ConcurrentQueue<Entry>();
        private Task _consumer;
        private IList<IPublisher> _publishers = new List<IPublisher>();
        private bool _running = false;

       public int TimeWait { get; set; } = 1000;

        public Logger(IList<IPublisher> publishers) : this()
        {
            _publishers = publishers;
        }

        public Logger()
        {
            if (_publishers.Count == 0)
            {
                _publishers.Add(new ConsolePublisher() 
                    { 
                        Formatter = new TextFormatter() 
                    }
                );
            }

            _running = true;
            _consumer = Task.Factory.StartNew(() => Consume());
        }

        private void Consume()
        {            
            Entry item;

            while (_running || !_entries.IsEmpty)
            {                    
                while (_entries.TryDequeue(out item))
                {
                    foreach (var publishers in _publishers)
                    {
                        publishers.Publish(item);
                    }
                }

                if (_running)
                {
                    Thread.Sleep(TimeWait);
                }
            }            
        }

        public void Log(Entry entry)
        {
            _entries.Enqueue(entry);
        }

        public void Log(LogLevel level, string source, string message, IDictionary<string, object> details = null)
        {
            _entries.Enqueue(new Entry() { Level = level, Source = source, Messsage = message, Details = details });
        }

        public void Dispose()
        {
            _running = false;
            _consumer.Wait();
        }
    }
}

