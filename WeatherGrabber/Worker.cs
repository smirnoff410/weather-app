using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.MessageQueue;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase.Specification;
using WeatherDatabase.Specification.Forecast;
using WeatherGrabber.Clients;
using WeatherGrabber.Models;
using WeatherGrabber.Services;
using WeatherGrabber.Services.Mappings;
using WeatherGrabber.Settings;

namespace WeatherGrabber
{
    public class Worker : BackgroundService
    {
        private readonly IWeatherClient _weatherClient;
        private readonly IWeatherGrabberMappingFactory _mappingFactory;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Forecast> _forecastRepository;
        private readonly IMessageQueue _messageQueue;
        private readonly ServiceSettings _serviceSettings;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IWeatherClient weatherClient,
            IWeatherGrabberMappingFactory mappingFactory,
            IRepository<City> cityRepository, 
            IRepository<Forecast> forecastRepository, 
            IMessageQueue messageQueue, 
            IOptions<ServiceSettings> serviceSettings, 
            ILogger<Worker> logger)
        {
            _weatherClient = weatherClient;
            _mappingFactory = mappingFactory;
            _cityRepository = cityRepository;
            _forecastRepository = forecastRepository;
            _messageQueue = messageQueue;
            _serviceSettings = serviceSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                var cities = await _cityRepository.Get().Select(x => new { x.Id, x.Name } ).ToListAsync(cancellationToken: stoppingToken);
                /*
                 Get unique user cities from database
                 */

                var forecastToGrabberModelMapper = _mappingFactory.GetMapper<Forecast, ForecastGrabberModel>();
                foreach (var city in cities)
                {
                    var forecasts = await _forecastRepository.Get(new GetForecastByCityIDSpecification(city.Id)).ToListAsync(cancellationToken: stoppingToken);

                    var result = await _weatherClient.GetForecast(city.Name);

                    var mapper = new ForecastComparerService();
                    var dbForecasts = forecasts.Select(forecastToGrabberModelMapper.Map);

                    //var apiForecasts = result.forecast.forecastday.Select(c => c.)
                    //var mapForecast = mapper.Map(result);

                    //forecasts.Equals(mapForecast);
                }
                

                var message = "Hello from RabbitMQ";
                _messageQueue.Publish(MessageQueueRouteEnum.WeatherChangeAlert, new WeatherChangeAlertRequest(1, message)); 
                Console.WriteLine("Sending message to my-queue-name");

                /*
                 Get forecast from database and compare with api
                if they not equal -> save new forecast to database and send message to telegram service
                 */


                await Task.Delay(TimeSpan.FromMinutes(_serviceSettings.WorkerIntervalMinutes), stoppingToken);
            }
        }
    }
}