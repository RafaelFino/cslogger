using CSLoggerLib;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerTesterConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filePublisher = new FilePublisher() 
            {
                Formatter = new TextFormatter()
            };

            filePublisher.Start(new Dictionary<string, object>() { 
                { 
                    FilePublisher.CONFIG_FILEPATH, 
                    string.Format("{0}{1}log", "c:\\temp", Path.DirectorySeparatorChar )}
            });

            var consolePublihser = new ConsolePublisher()
            {
                Formatter = new TextFormatter()
            };

            consolePublihser.Start();

            IList<IPublisher> publishers = new List<IPublisher>(){
                filePublisher,
                consolePublihser,
            };

            using (var logger = new Logger(publishers))
            {
                logger.Log(LogLevel.Info, "ConsoleTest", "Estou vivooooo!!!");
                logger.Log(LogLevel.Info, "ConsoleTest", "Ainda estou vivooooo!!!");

                for (int i = 0; i < 10; i++)
                {
                    var id = "Thread " + i.ToString();
                    Task.Factory.StartNew(() => Run(logger, id, 200));
                }                

                logger.Log(LogLevel.Info, "ConsoleTest", "Vou morrer ...");
            }
        }

        static void Run(Logger logger, string id, int qty)
        {
            for (int i = 0; i < qty; i++)
            {
                logger.Log(LogLevel.Debug, id, "Mensagem de número " + i.ToString());
            }
        }
    }
}
