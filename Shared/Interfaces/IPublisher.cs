using System.Collections.Generic;

namespace Commons.Interfaces
{
    public interface IPublisher<T>
    {
        public void Start();

        public void Stop();

        public void Publish(T message);

        public void Publish(IEnumerable<T> messages);
    }
}
