using Microsoft.EntityFrameworkCore;
using WeatherCommon.Services.MessageQueue;
using WeatherGrabber.Clients;
using WeatherGrabber.Settings;
using WeatherDatabase;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherGrabber.Services.Mappings;
using System.Text.Json;
using WeatherTelegramService.Settings;

namespace WeatherGrabber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((configuration, services) =>
                {
                    services.AddProblemDetails();
                    services.ConfigureOpenTelemetryExporters();
                    services.ConfigureOpenTelemetry();
                    services.AddSingleton<IWeatherGrabberMappingFactory, WeatherGrabberMappingFactory>();
                    services.AddHttpClient("weatherapi", client => client.BaseAddress = new Uri("https://api.weatherapi.com/v1/"));
                    services.AddSingleton<IWeatherClient>(x => new WeatherApiClient(x.GetRequiredService<IHttpClientFactory>(), GetWeatherApiKeyFromSecrets()));
                    services.Configure<ServiceSettings>(configuration.Configuration.GetSection("ServiceSettings"));
                    services.AddSingleton<IMessageQueue>(x => 
                        new RabbitMessageQueue(configuration.Configuration.GetSection("QueueSettings").GetSection("ConnectionString").Value,
                        x.GetRequiredService<ILogger<IMessageQueue>>()));

                    services.AddDbContext<WeatherDatabaseContext>(options =>
                        options.UseSqlServer(configuration.Configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value));
                    services.AddScoped<IRepository<City>>(x => new Repository<City>(x.GetRequiredService<WeatherDatabaseContext>()));
                    services.AddScoped<IRepository<User>>(x => new Repository<User>(x.GetRequiredService<WeatherDatabaseContext>()));
                    services.AddScoped<IRepository<Forecast>>(x => new Repository<Forecast>(x.GetRequiredService<WeatherDatabaseContext>()));

                    services.AddHostedService<Worker>();

                    var provider = services.BuildServiceProvider();
                    provider.GetRequiredService<IWeatherGrabberMappingFactory>().Initialize();
                })
                .Build();

            host.Run();
        }
        

        private static string GetWeatherApiKeyFromSecrets()
        {
            var text = File.ReadAllText("secrets.json");
            var content = JsonSerializer.Deserialize<SecretSettings>(text);

            return content is null ? string.Empty : content.WeatherApiKey;
        }
    }
}