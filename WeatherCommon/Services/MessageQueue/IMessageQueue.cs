using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.Command;

namespace WeatherCommon.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void Publish<T>(MessageQueueRouteEnum routingKey, T data);
        void Subscribe<T>(MessageQueueRouteEnum routingKey, ICommand<T> handler);
    }
}
