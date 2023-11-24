using Telegram.Bot;
using WeatherCommon.Services.MessageQueue;
using WeatherTelegramService.Handlers;
using WeatherTelegramService.Services.Telegram;
using WeatherTelegramService.Settings;
using System.Text.Json;
using WeatherCommon.Services.Command;
using WeatherCommon.Models.Request;
using WeatherTelegramService.Services.ServiceBuilder;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

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
                    services.AddScoped<ITelegramReceiverService, TelegramReceiverService>();
                    services.AddScoped<ICommand<WeatherChangeAlertRequest>, WeatherChangeAlertCommand>();

                    services.AddDbContext<WeatherDatabaseContext>(options =>
                        options.UseSqlServer(configuration.Configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value)); ;
                    services.AddScoped<IRepository<City>>(x => new Repository<City>(x.GetRequiredService<WeatherDatabaseContext>()));
                    services.AddScoped<IRepository<User>>(x => new Repository<User>(x.GetRequiredService<WeatherDatabaseContext>()));

                    ServiceProviderBuilder spBuilder = new(services);
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