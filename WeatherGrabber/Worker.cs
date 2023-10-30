using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.MessageQueue;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase.Specification;
using WeatherGrabber.Clients;
using WeatherGrabber.Settings;

namespace WeatherGrabber
{
    public class Worker : BackgroundService
    {
        private readonly IWeatherClient _weatherClient;
        private readonly IRepository<City> _cityRepository;
        private readonly IMessageQueue _messageQueue;
        private readonly ServiceSettings _serviceSettings;
        private readonly ILogger<Worker> _logger;

        public Worker(IWeatherClient weatherClient, IRepository<City> cityRepository, IMessageQueue messageQueue, IOptions<ServiceSettings> serviceSettings, ILogger<Worker> logger)
        {
            _weatherClient = weatherClient;
            _cityRepository = cityRepository;
            _messageQueue = messageQueue;
            _serviceSettings = serviceSettings.Value;
            _logger = logger;
        }

        public class GetUniqueCitiesSpecification : Specification<City>
        {
            public GetUniqueCitiesSpecification() : base(x => true)
            {
                AddInclude(x => x.Users);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var cities = _cityRepository.Get(new GetUniqueCitiesSpecification()).Select(x => x.Name).Distinct().ToListAsync();
                /*
                 Get unique user cities from database
                 */

                //var result = await _weatherClient.GetForecast("London");

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