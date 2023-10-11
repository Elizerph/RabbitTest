namespace RabbitTest
{
    public interface IQueueService : IDisposable
    {
        ISender GetQueueSender();
        IListener GetQueueListener();
    }
}
