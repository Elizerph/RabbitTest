using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;

namespace RabbitTest
{
    internal class RabbitListener : IListener
    {
        private readonly IModel _channel;
        private readonly string _queue;

        public RabbitListener(IModel channel, string queue)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public async Task Listen(Func<string, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                await action(Encoding.UTF8.GetString(ea.Body.ToArray()), cancellationToken);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_queue, false, consumer);
            await Task.Delay(-1, cancellationToken);
        }
    }
}
