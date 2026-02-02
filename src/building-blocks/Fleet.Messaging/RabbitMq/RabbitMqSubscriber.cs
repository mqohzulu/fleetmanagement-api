using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Fleet.Messaging.Abstractions;

namespace Fleet.Messaging.RabbitMq
{
    public class RabbitMqSubscriber<T> where T : class
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly Func<T, Task> _handler;

        public RabbitMqSubscriber(RabbitMqOptions options, string queueName, Func<T, Task> handler)
        {
            var factory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(options.ExchangeName, ExchangeType.Fanout, durable: true);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, options.ExchangeName, string.Empty);
            _handler = handler;
        }

        public void Start()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(body);
                if (message != null)
                {
                    await _handler(message);
                }
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
        }

        public void Stop()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
