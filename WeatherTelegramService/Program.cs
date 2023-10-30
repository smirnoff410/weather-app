using Telegram.Bot;
using WeatherCommon.Services.MessageQueue;
using WeatherTelegramService.Handlers;
using WeatherTelegramService.Services.Telegram;
using WeatherTelegramService.Settings;
using System.Text.Json;
using WeatherCommon.Services.Command;
using WeatherCommon.Models.Request;
using WeatherTelegramService.Services.ServiceBuilder;

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
                    services.AddSingleton<ITelegramBotClient>(x => new TelegramBotClient(GetTelegramBotTokenFromSecrets()));
                    services.AddSingleton<ITelegramReceiverService, TelegramReceiverService>();
                    services.AddScoped<ICommand<WeatherChangeAlertRequest>, WeatherChangeAlertHandler>();

                    using ServiceProviderBuilder spBuilder = new(services);
                    spBuilder
                        .AddTelegramReceiverService()
                        .AddMessageQueue()
                        .Build();
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