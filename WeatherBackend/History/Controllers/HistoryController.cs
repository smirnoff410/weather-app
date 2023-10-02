using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.History.Controllers
{
    using System;
    using WeatherBackend.Services.WeatherService;
    using WeatherBackendAfter.History.Models;

    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public HistoryController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpGet("")]
        public IEnumerable<WeatherHistory> GetHistory(DateTime fromDate, DateTime toDate)
        {
            return _weatherService.GenerateHistory(fromDate, toDate);
        }
    }
}
