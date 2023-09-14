using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.History.Controllers
{
    using System;
    using WeatherBackendAfter.History.Models;
    using WeatherBackendAfter.Services.WeatherService;

    public class HistoryController : ControllerBase
    {
        [HttpGet("")]
        public IEnumerable<WeatherHistory> GetHistory(DateTime fromDate, DateTime toDate)
        {
            return new WeatherService().GenerateHistory(fromDate, toDate);
        }
    }
}
