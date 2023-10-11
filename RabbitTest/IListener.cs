namespace RabbitTest
{
    public interface IListener
    {
        Task Listen(Func<string, CancellationToken, Task> action, CancellationToken cancellationToken = default);
    }
}
