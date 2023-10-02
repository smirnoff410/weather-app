namespace WeatherBackend.WeatherForecast.Models
{
    using City.Models;
    using WeatherBackendAfter.History.Models;

    public class WeatherForecastFuture : WeatherHistory
    {
        public City City { get; set; }
    }
}
