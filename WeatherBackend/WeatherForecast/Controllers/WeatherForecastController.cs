using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.WeatherForecast.Controllers
{
    using WeatherBackend.City.Repository;
    using WeatherBackend.Services.WeatherService;
    using WeatherBackend.WeatherForecast.Models;
    using WeatherBackendAfter.Services.WeatherService;
    using WeatherBackendAfter.WeatherForecast.Models;
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ICityRepository _cityRepository;
        public WeatherForecastController(ICityRepository cityRepository)
        {
            _weatherService = new WeatherService();
            _cityRepository = cityRepository;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecastFuture> ListFuture()
        {
            var response = new List<WeatherForecastFuture>();
            foreach(var city in _cityRepository.List())
            {
                var forecasts = _weatherService.GenerateHistory(DateTime.UtcNow, DateTime.UtcNow.AddDays(3)).ToList();
                foreach(var forecast in forecasts)
                {
                    response.Add(new WeatherForecastFuture { City = city, Date = forecast.Date, MaxTemperature = forecast.MaxTemperature, MinTemperature = forecast.MinTemperature });
                }
            }
            return response;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> Current()
        {
            return _weatherService.GenerateForCurrentDay(24).ToArray();
        }
    }
}