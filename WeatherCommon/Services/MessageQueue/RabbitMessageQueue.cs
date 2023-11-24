using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.Command;

namespace WeatherCommon.Services.MessageQueue
{
    public class RabbitMessageQueue : IMessageQueue
    {
        private readonly IModel _channel;

        public RabbitMessageQueue()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }
        public void Publish<T>(MessageQueueRouteEnum routingKey, T data)
        {
            var dataString = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(dataString);
            _channel.BasicPublish(string.Empty, routingKey.ToString(), null, body);
        }

        public void Subscribe<T>(MessageQueueRouteEnum routingKey, ICommand<T> command)
        {
            var queueName = _channel.QueueDeclare(routingKey.ToString(), false, false, false, null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<T>(message);
                if (data != null)
                    command.Execute(data);
            };
            _channel.BasicConsume(routingKey.ToString(), true, consumer);
        }
    }
}
