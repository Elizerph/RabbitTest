using RabbitMQ.Client;

namespace RabbitTest
{
    public class RabbitService : IQueueService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queue;

        public RabbitService(string? queue = null)
        {
            _queue = queue ?? "default";
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass,
                DispatchConsumersAsync = true
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_queue, false, false, false);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        public IListener GetQueueListener()
        {
            return new RabbitListener(_channel, _queue);
        }

        public ISender GetQueueSender()
        {
            return new RabbitSender(_channel, _queue);
        }
    }
}
