using RabbitMQ.Client;

using System.Text;

namespace RabbitTest
{
    public class RabbitSender : ISender
    {
        private readonly IModel _channel;
        private readonly string _queue;

        public RabbitSender(IModel channel, string queue)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(string.Empty, _queue, null, body);
        }
    }
}
