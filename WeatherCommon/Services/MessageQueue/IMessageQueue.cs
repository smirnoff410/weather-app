using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.Handlers;

namespace WeatherCommon.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void Publish<T>(MessageQueueRouteEnum routingKey, T data);
        void Subscribe<T>(MessageQueueRouteEnum routingKey, IBaseHandler<T> handler);
    }
}
