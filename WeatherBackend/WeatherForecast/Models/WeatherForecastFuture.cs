namespace WeatherBackend.WeatherForecast.Models
{
    using City.Models;
    using WeatherBackend.History.Models;
    using WeatherBackend.Models;

    public class WeatherForecastFuture : WeatherHistory
    {
        public string City { get; set; }
    }
}
