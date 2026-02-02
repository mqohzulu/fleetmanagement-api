using Fleet.Messaging.Abstractions;

#if RABBITMQ
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Fleet.Messaging.RabbitMq
{
    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMqOptions _options;

        public RabbitMqEventBus(RabbitMqOptions options)
        {
            _options = options;
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_options.ExchangeName, ExchangeType.Fanout, durable: true);
        }

        public Task PublishAsync<T>(T @event) where T : class
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(@event);
            _channel.BasicPublish(exchange: _options.ExchangeName, routingKey: string.Empty, basicProperties: null, body: body);
            return Task.CompletedTask;
        }

        public void Publish<T>(T @event) where T : class
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(@event);
            _channel.BasicPublish(exchange: _options.ExchangeName, routingKey: string.Empty, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();
            }
            catch { }
        }
    }
}
#else
namespace Fleet.Messaging.RabbitMq
{
    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        public RabbitMqEventBus(RabbitMqOptions options) { }
        public Task PublishAsync<T>(T @event) where T : class => Task.CompletedTask;
        public void Publish<T>(T @event) where T : class { }
        public void Dispose() { }
    }
}
#endif
