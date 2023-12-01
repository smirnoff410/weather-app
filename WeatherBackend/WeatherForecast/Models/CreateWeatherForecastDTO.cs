namespace WeatherBackend.WeatherForecast.Models
{
    public class CreateWeatherForecastDTO
    {
        public int Temperature { get; set; }
        public string Summary { get; set; }
        public Guid CityID { get; set; }
    }
}
