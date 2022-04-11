using Commons.Interfaces;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Receiver
{
    public abstract class Receiver<T> : IDisposable, IReceiver<T>
    {
        private readonly BlockingCollection<T> _data = new BlockingCollection<T>();
        protected IEnumerable<IPublisher<T>> _publishers = new List<IPublisher<T>>();
        protected IReceiverConfig _config;
        private bool disposedValue;
        private bool _stopRequest = false;

        private void PublishData()
        {           
            while (!_stopRequest || _data.Count > 0)
            {
                T msg;
                if (_data.TryTake(out msg, _config.MessageTimeOut))
                {
                    foreach (var pub in _publishers)
                    {
                        pub.Publish(msg);
                    }
                }

                if ( _data.Count == 0)
                {
                    Thread.Sleep(_config.MessageTimeOut);
                }
            }
        }

        public void Start()
        {
            foreach (var pub in _publishers)
            {
                pub.Start();
            }

            if(_publishers.Count() > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    PublishData();
                });
            }
        }

        public void Stop()
        {
            _stopRequest = true;

            while (_data.Count > 0)
            {
                Thread.Sleep(_config.MessageTimeOut);
            }
        }

        #region Dispose
        ~Receiver()
        {
            Dispose();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();

                    foreach (var pub in _publishers)
                    {
                        pub.Stop();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public void Send(T data)
        {
            _data.Add(data);
        }

        public void Send(IEnumerable<T> data)
        {
            foreach (var item in data)
            {
                _data.Add(item);    
            }            
        }        
    }
}
