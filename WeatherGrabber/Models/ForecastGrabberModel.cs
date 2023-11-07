namespace WeatherGrabber.Models
{
    public class ForecastGrabberModel
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; } = null!;
    }
}
