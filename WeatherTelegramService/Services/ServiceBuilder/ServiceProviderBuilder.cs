using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherCommon.Services.MessageQueue;
using WeatherTelegramService.Services.Telegram;

namespace WeatherTelegramService.Services.ServiceBuilder
{
    public class ServiceProviderBuilder : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceProviderBuilder(IServiceCollection services)
        {
            _serviceProvider = services.BuildServiceProvider();
        }
        public ServiceProviderBuilder AddTelegramReceiverService()
        {
            var telegramSender = _serviceProvider.GetRequiredService<ITelegramReceiverService>();
            telegramSender.Initialize();
            return this;
        }
        public ServiceProviderBuilder AddMessageQueue()
        {
            var messageQueue = _serviceProvider.GetRequiredService<IMessageQueue>();
            messageQueue.Subscribe(MessageQueueRouteEnum.WeatherChangeAlert, _serviceProvider.GetRequiredService<ICommand<WeatherChangeAlertRequest>>());
            return this;
        }

        public ServiceProvider Build()
        {
            return _serviceProvider;
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}
