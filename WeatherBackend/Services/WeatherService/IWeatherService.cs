using WeatherBackend.History.Models;

namespace WeatherBackend.Services.WeatherService
{
    using WeatherBackend.WeatherForecast.Models;
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GenerateForCurrentDay(int toHours);
        IEnumerable<WeatherHistory> GenerateHistory(DateTime fromDate, DateTime toDate);
    }
}
