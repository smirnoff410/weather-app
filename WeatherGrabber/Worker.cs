using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.MessageQueue;
using WeatherDatabase;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase.Specification.Forecast;
using WeatherGrabber.Clients;
using WeatherGrabber.Models;
using WeatherGrabber.Services.Mappings;
using WeatherGrabber.Settings;

namespace WeatherGrabber
{
    public class Worker : BackgroundService
    {
        private readonly IWeatherClient _weatherClient;
        private readonly IWeatherGrabberMappingFactory _mappingFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageQueue _messageQueue;
        private readonly ServiceSettings _serviceSettings;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IWeatherClient weatherClient,
            IWeatherGrabberMappingFactory mappingFactory,
            IServiceProvider serviceProvider,
            IMessageQueue messageQueue, 
            IOptions<ServiceSettings> serviceSettings, 
            ILogger<Worker> logger)
        {
            _weatherClient = weatherClient;
            _mappingFactory = mappingFactory;
            _serviceProvider = serviceProvider;
            _messageQueue = messageQueue;
            _serviceSettings = serviceSettings.Value;
            _logger = logger;

            _mappingFactory.Initialize();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var scope = _serviceProvider.CreateScope();
                    var cityRepository = scope.ServiceProvider.GetRequiredService<IRepository<City>>();
                    var forecastRepository = scope.ServiceProvider.GetRequiredService<IRepository<Forecast>>();
                    var context = scope.ServiceProvider.GetRequiredService<WeatherDatabaseContext>();

                    var cities = await cityRepository.Get().Select(x => new { x.Id, x.Name }).ToListAsync(cancellationToken: stoppingToken);
                    /*
                     Get unique user cities from database
                     */

                    var forecastToGrabberModelMapper = _mappingFactory.GetMapper<Forecast, ForecastGrabberModel>();
                    var forecastDtoToGrabberModelMapper = _mappingFactory.GetMapper<DTO.WeatherAPI.Hour, ForecastGrabberModel>();
                    var forecastGrabberModelToForecastMapper = _mappingFactory.GetMapper<ForecastGrabberModel, Forecast>();
                    foreach (var city in cities)
                    {
                        var forecasts = await forecastRepository.Get(new GetForecastByCityIDSpecification(city.Id)).ToListAsync(cancellationToken: stoppingToken);

                        var result = await _weatherClient.GetForecast(city.Name);

                        var dbForecasts = forecasts.Select(forecastToGrabberModelMapper.Map);

                        var apiForecasts = result.forecast.forecastday.First().hour;
                        var mapForecast = apiForecasts.Select(forecastDtoToGrabberModelMapper.Map);

                        var equal = dbForecasts.SequenceEqual(mapForecast);

                        if (!equal)
                        {
                            var message = "Weather changed, look at the window!";
                            _messageQueue.Publish(MessageQueueRouteEnum.WeatherChangeAlert, new WeatherChangeAlertRequest(city.Id, message));
                            _logger.LogInformation("Sending message to {0} queue", MessageQueueRouteEnum.WeatherChangeAlert);

                            foreach(var dbForecast in forecasts)
                            {
                                forecastRepository.Delete(dbForecast);
                            }

                            var newForecast = mapForecast.Select(forecastGrabberModelToForecastMapper.Map).ToList();
                            foreach(var f in newForecast)
                            {
                                f.CityId = city.Id;
                                f.Id = Guid.NewGuid();
                                await forecastRepository.Add(f);
                            }

                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }

                    /*
                     Get forecast from database and compare with api
                    if they not equal -> save new forecast to database and send message to telegram service
                     */

                    await Task.Delay(TimeSpan.FromMinutes(_serviceSettings.WorkerIntervalMinutes), stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation("Worker did end with exception: {0}", ex.Message);
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}