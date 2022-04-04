namespace Commons.Interfaces
{
    public interface IPublisher<T>
    {
        public void Start();

        public void Stop();

        public void Publish(T message);
    }
}
