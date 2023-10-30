namespace WeatherBackend.WeatherForecast.Models
{
    using WeatherBackend.History.Models;

    public class WeatherForecastFuture : WeatherHistory
    {
        public string City { get; set; }
    }
}
