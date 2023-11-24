namespace WeatherGrabber.Models
{
    public class ForecastGrabberModel : IEquatable<ForecastGrabberModel>
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; } = null!;

        public bool Equals(ForecastGrabberModel? other)
        {
            if(other is null)
                return false;

            return Date == other.Date && Temperature == other.Temperature && Condition == other.Condition;
        }
    }
}
