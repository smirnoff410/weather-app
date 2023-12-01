namespace WeatherBackend.History.Models
{
    public class WeatherHistory
    {
        public DateOnly Date { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public EWeatherForecastType Type 
        { 
            get 
            {
                var midTemperature = (MinTemperature + MaxTemperature) / 2;
                return midTemperature <= 0
                    ? EWeatherForecastType.Home : midTemperature > 0 && midTemperature <= 25
                    ? EWeatherForecastType.Walk : EWeatherForecastType.Swim;
            }
        }
    }
}
