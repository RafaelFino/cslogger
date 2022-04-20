using Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Commons.Publishers
{
    public class FilePublisher<T> : IPublisher<T>, IDisposable
    {        
        private readonly string _path;
        private DateTime _lastOpen;
        private string _fileName;
        private readonly string _baseFileName;
        private readonly string _fileExtension;
        private readonly IFormatter<T> _formatter;
        private StringBuilder[] _buff = { new StringBuilder(), new StringBuilder() };
        private int _currBuff = 0;
        private int _flushInterval = 1;
        private System.Timers.Timer _timer;
        public const string TagPath = "path";
        public const string TagFilename = "filename";
        public const string TagExtensionFilename = "filename-extension";
        public const string TagFlushInterval = "flush-interval";

        public static IDictionary<string, object> CreateDefaultConfig(string name)
        {
            var ret = new Dictionary<string, object>
            {
                { TagPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs") },
                { TagFilename, string.Format($"{AppDomain.CurrentDomain.FriendlyName}-{name}") },
                { TagExtensionFilename, "log" },
                { TagFlushInterval, 1000 }
            };

            return ret;
        }

        public FilePublisher(IDictionary<string, object> config, IFormatter<T> formatter)
        {
            _path = (string)config[TagPath];
            _baseFileName = (string)config[TagFilename];
            _fileExtension = (string)config[TagExtensionFilename];

            _flushInterval = (int)config[TagFlushInterval];

            _formatter = formatter;
        }

        private string GetFileName()
        {
            if (string.IsNullOrEmpty(_fileName) || _lastOpen.Day != DateTime.Now.Day)
            {
                _fileName = Path.Combine(_path, string.Format($"{DateTime.Now.ToString("yyyyMMdd")}_{_baseFileName}.{_fileExtension}"));
            }

            return _fileName;
        }

        public void Publish(T message)
        {
            var msg = _formatter.Format(message);
            GetCurrent().AppendLine(msg);
        }

        public void Publish(IEnumerable<T> messages)
        {
            foreach(var m in messages)
            {
                this.Publish(m);
            }
        }        

        public void Start()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            _lastOpen = DateTime.Now;

            _timer = new System.Timers.Timer(_flushInterval);
            _timer.Elapsed += FlushTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void FlushTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Flush();
        }

        public void Stop()
        {
            _timer.Stop();
            Flush(true);
        }

        private StringBuilder GetCurrent()
        {
            return _buff[_currBuff];
        }

        private void Flush(bool all = false)
        {
            if (all)
            {
                var result = new StringBuilder();

                foreach (var b in _buff)
                {
                    if (b.Length > 0)
                    {
                        result.Append(b);
                        b.Clear();
                    }
                }

                File.AppendAllText(GetFileName(), result.ToString());
            }
            else
            {
                var buff = GetCurrent();
                if (buff.Length > 0)
                {
                    if (_currBuff >= _buff.Length - 1)
                    {
                        _currBuff = 0;
                    }
                    else
                    {
                        _currBuff++;
                    }

                    File.AppendAllText(GetFileName(), buff.ToString());
                    buff.Clear();
                }
            }
        }

        public void Dispose()
        {
            Flush(true);
        }
    }
}
