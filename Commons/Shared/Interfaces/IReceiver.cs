namespace Commons.Interfaces
{
    public interface IReceiver<T>
    {
        void Dispose();
        void Publish(T data);
        void Start();
        void Stop();
    }
}