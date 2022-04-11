using System.Collections.Generic;

namespace Commons.Interfaces
{
    public interface IReceiver<T>
    {
        void Dispose();

        void Send(T data);

        void Send(IEnumerable<T> data);
        void Start();
        void Stop();
    }
}