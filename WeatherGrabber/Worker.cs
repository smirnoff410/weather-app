using Microsoft.Extensions.Options;
using WeatherCommon.Models.MessageQueue;
using WeatherCommon.Services.MessageQueue;
using WeatherGrabber.Clients;
using WeatherGrabber.Settings;

namespace WeatherGrabber
{
    public class Worker : BackgroundService
    {
        private readonly IWeatherClient _weatherClient;
        private readonly IMessageQueue _messageQueue;
        private readonly ServiceSettings _serviceSettings;
        private readonly ILogger<Worker> _logger;

        public Worker(IWeatherClient weatherClient, IMessageQueue messageQueue, IOptions<ServiceSettings> serviceSettings, ILogger<Worker> logger)
        {
            _weatherClient = weatherClient;
            _messageQueue = messageQueue;
            _serviceSettings = serviceSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                /*
                 Get unique user cities from database
                 */

                //var result = await _weatherClient.GetForecast("London");

                var message = "Hello from RabbitMQ";
                _messageQueue.Publish(MessageQueueRouteEnum.WeatherChangeAlert, message);
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