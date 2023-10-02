using WeatherBackendAfter.History.Models;

namespace WeatherBackend.Services.WeatherService
{
    using WeatherBackendAfter.WeatherForecast.Models;
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GenerateForCurrentDay(int toHours);
        IEnumerable<WeatherHistory> GenerateHistory(DateTime fromDate, DateTime toDate);
    }
}
