using Microsoft.AspNetCore.Mvc;
using System;
using WeatherBackendBefore;

namespace WeatherBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Dictionary<Guid, string> _cities = new()
        {
            { Guid.NewGuid(), "Volgograd" },
            { Guid.NewGuid(), "Moscow" },
            { Guid.NewGuid(), "Saint-Petersburg" },
            { Guid.NewGuid(), "Ekaterinburg" },
            { Guid.NewGuid(), "Vladivostok" },
            { Guid.NewGuid(), "Kaliningrad" }
        };
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(int toIndex)
        {
            var result = Enumerable.Range(1, toIndex).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddHours(index),
                Temperature = Random.Shared.Next(-20, 45),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return result;
        }

        [HttpGet("[action]")]
        public IEnumerable<HistoryWeather> GetHistory(DateTime fromDate, DateTime toDate)
        {
            var days = toDate - fromDate;
            var result = Enumerable.Range(1, days.Days).Select(index => new HistoryWeather
            {
                Date = DateOnly.FromDateTime(fromDate.AddDays(index)),
                MinTemperature = Random.Shared.Next(-20, 15),
                MaxTemperature = Random.Shared.Next(15, 45)
            })
            .ToArray();

            foreach (var item in result)
            {
                var a = (item.MinTemperature + item.MaxTemperature) / 2;
                if (a <= 0)
                    item.Type = EWeatherForecastType.Home;
                if (a > 0 && a <= 25)
                    item.Type = EWeatherForecastType.Walk;
                if (a > 25)
                    item.Type = EWeatherForecastType.Swim;
            }

            return result;
        }

        [HttpGet("[action]")]
        public Dictionary<Guid, string> GetCities()
        {
            return _cities;
        }
    }
}