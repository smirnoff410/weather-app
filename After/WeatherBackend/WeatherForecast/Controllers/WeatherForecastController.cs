using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.WeatherForecast.Controllers
{
    using WeatherBackendAfter.Services.WeatherService;
    using WeatherBackendAfter.WeatherForecast.Models;
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> Current()
        {
            return new WeatherService().GenerateForCurrentDay(24).ToArray();
        }
    }
}