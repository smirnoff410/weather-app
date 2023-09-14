using WeatherBackend;

namespace WeatherBackendBefore
{
    public class HistoryWeather
    {
        public DateOnly Date { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public EWeatherForecastType Type { get; set; }
    }
}
