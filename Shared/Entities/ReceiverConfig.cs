using Commons.Enum;
using Commons.Interfaces;

namespace Commons.Entities
{
    public abstract class ReceiverConfig : IReceiverConfig
    {
        public int MessageTimeOut { get; set; } = 500;
    }
}