using Telegram.Bot;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.Handlers;
using WeatherCommon.Services.MessageQueue;
using WeatherTelegramService.Handlers;
using WeatherTelegramService.Services.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using WeatherTelegramService.Settings;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherTelegramService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((configuration, services) =>
                {
                    services.AddSingleton<IMessageQueue, RabbitMessageQueue>();
                    services.AddSingleton<ITelegramReceiverService, TelegramReceiverService>();
                    
                    services.AddSingleton<ITelegramBotClient>(x => new TelegramBotClient(GetTelegramBotTokenFromSecrets()));
                    services.AddScoped<IBaseHandler<string>, WeatherChangeAlertHandler>();

                    using var serviceProvider = services.BuildServiceProvider();
                    var telegramSender = serviceProvider.GetRequiredService<ITelegramReceiverService>();
                    telegramSender.Initialize();
                    var messageQueue = serviceProvider.GetRequiredService<IMessageQueue>();
                    messageQueue.Subscribe(MessageQueueRouteEnum.WeatherChangeAlert, serviceProvider.GetRequiredService<IBaseHandler<string>>());
                })
                .Build();

            
            host.Run();
        }

        private static string GetTelegramBotTokenFromSecrets()
        {
            var text = File.ReadAllText("secrets.json");
            var content = JsonSerializer.Deserialize<SecretSettings>(text);

            return content is null ? string.Empty : content.TelegramBotToken;
        }
    }
}