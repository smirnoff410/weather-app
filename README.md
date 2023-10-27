# Лабораторная работа №4
## RabbitMQ

### Определение
RabbitMQ - это брокер сообщений, позволяющий отправлять и принимать сообщения в различных компонентах вашей системы. RabbitMQ использует свой собственный протокол AMQP который работает поверх TCP/IP. Официальный сайт, на котором есть соответствующая документация и примеры кода на разных языках находится по адресу https://www.rabbitmq.com/

### Шаги
1)	Установить и настроить RabbitMQ на локальном компьютере
2)	Познакомиться с Web-интерфейсом брокера сообщений
3)	Познакомиться с основными обменниками(Exchanges) RabbitMQ:
- Default Exchange
- Direct Exchange
- Topic Exchange
- Fanout Exchange

### Пример
![Alt text](image.png)
Инициализация подключения к брокеру лежит в WeatherCommon.Services.MessageQueue.RabbitMessageQueue
```csharp
public class RabbitMessageQueue : IMessageQueue
{
    private readonly IModel _channel;

    public RabbitMessageQueue()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://weather_user:weather_user@localhost:5672")
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

    public void Subscribe<T>(MessageQueueRouteEnum routingKey, IBaseHandler<T> handler)
    {
        var queueName = _channel.QueueDeclare(routingKey.ToString(), false, false, false, null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<T>(message);
            if (data != null)
                handler.Handle(data);
        };
        _channel.BasicConsume(routingKey.ToString(), true, consumer);
    }
}
```
В конструкторе происходит подключение к RabbitMQ и реализуются базовые методы для публикации сообщения в очередь и подписки на сообщения

В проекте WeatherGrabber происходит отправка сообщения
Код из класса WeatherGrabber.Worker
```csharp
var message = "Hello from RabbitMQ";
_messageQueue.Publish(MessageQueueRouteEnum.WeatherChangeAlert, message);
```
В проекте WeatherTelegramService происходит подписка на сообщения из очереди
Код из класса WeatherTelegramService.Program
```csharp
messageQueue.Subscribe(MessageQueueRouteEnum.WeatherChangeAlert, serviceProvider.GetRequiredService<IBaseHandler<string>>());
```
### Задание
Внедрить в свою систему брокер сообщений для взаимодействия между компонентами приложения

#### Методичка по RabbitMQ лежит в корне репозитория

