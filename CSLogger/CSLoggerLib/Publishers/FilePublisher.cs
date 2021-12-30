using System;
using System.Collections.Generic;
using System.IO;

namespace CSLoggerLib
{
    public class FilePublisher : IPublisher
    {
        private DateTime _last;
        private string _baseFileName = "log";
        private StreamWriter _sw;

        public const string CONFIG_FILEPATH = "FILEPATH";

        public IList<string> ConfigTags { get; set; } = new List<string>()
        {
            CONFIG_FILEPATH
        };
        public IEntryFormatter Formatter { get; set; }

        public void Publish(Entry entry)
        {
            if (_last.Date < DateTime.Now)
            {
                OpenFile();
            }

            string line = Formatter.Format(entry);
            _sw.WriteLine(line);
        }

        public void Start(IDictionary<string, object> config = null)
        {
            if (config != null)
            {
                object configFileName;
                if (config.TryGetValue(CONFIG_FILEPATH, out configFileName))
                {
                    _baseFileName = configFileName.ToString();
                }
            }
            OpenFile();

            return;
        }

        private void OpenFile()
        {
            if(_sw != null)
            {
                _sw.Close();
            }

            var path = GetFileName(_baseFileName);
            _sw = File.AppendText(path);
        }

        private string GetFileName(string fileName)
        {
            _last = DateTime.Now;
            return string.Format("{0}_{1}{2}{3}.log", fileName, _last.Year, _last.Month, _last.Day );
        }

        public void Stop()
        {
            if (_sw != null)
            {
                _sw.Close();
            }
        }
    }
}
