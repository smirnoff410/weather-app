using Microsoft.EntityFrameworkCore;
using WeatherCommon.Services.MessageQueue;
using WeatherGrabber.Clients;
using WeatherGrabber.Settings;
using WeatherDatabase;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;

namespace WeatherGrabber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((configuration, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddHttpClient("weatherapi", client => client.BaseAddress = new Uri("https://api.weatherapi.com/v1/"));
                    services.AddSingleton<IWeatherClient, WeatherApiClient>();
                    services.Configure<ServiceSettings>(configuration.Configuration.GetSection("ServiceSettings"));
                    services.AddSingleton<IMessageQueue, RabbitMessageQueue>();

                    services.AddDbContext<WeatherDatabaseContext>();
                    services.AddScoped<IRepository<City>>(x => new Repository<City>(x.GetRequiredService<WeatherDatabaseContext>()));
                    services.AddScoped<IRepository<User>>(x => new Repository<User>(x.GetRequiredService<WeatherDatabaseContext>()));
                })
                .Build();

            host.Run();
        }
    }
}