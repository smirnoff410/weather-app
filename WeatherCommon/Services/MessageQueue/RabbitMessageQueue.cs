using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.Command;
using Microsoft.Extensions.Logging;

namespace WeatherCommon.Services.MessageQueue
{
    public class RabbitMessageQueue : IMessageQueue
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;

        public RabbitMessageQueue(string? connectionString, ILogger<IMessageQueue> logger)
        {
            connectionString ??= "amqp://guest:guest@localhost:5672";

            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _logger = logger;
        }
        public void Publish<T>(MessageQueueRouteEnum routingKey, T data)
        {
            var dataString = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(dataString);
            _channel.BasicPublish(string.Empty, routingKey.ToString(), null, body);
            _logger.LogInformation("Sending message to {0} queue. Body: {1}", routingKey, dataString);
        }

        public void Subscribe<T>(MessageQueueRouteEnum routingKey, ICommand<T> command)
        {
            var queueName = _channel.QueueDeclare(routingKey.ToString(), false, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Get message from {0} queue. Body: {1}", routingKey, message);
                var data = JsonSerializer.Deserialize<T>(message);
                if (data != null)
                    command.Execute(data);
            };
            _channel.BasicConsume(routingKey.ToString(), true, consumer);
        }
    }
}
