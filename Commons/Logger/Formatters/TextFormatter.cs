using Commons.Interfaces;

namespace Logger.Formatters
{
    public class TextFormatter : IFormatter<ILogMessage>
    {
        public string Format(ILogMessage message)
        {
            if (message != null)
            {
                return message.ToString();
            }

            return string.Empty;
        }
    }
}
