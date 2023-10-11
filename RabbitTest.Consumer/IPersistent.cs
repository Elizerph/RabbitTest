namespace RabbitTest.Consumer
{
    public interface IPersistent<T> : IAsyncDisposable
    {
        Task Save(T item);
    }
}
