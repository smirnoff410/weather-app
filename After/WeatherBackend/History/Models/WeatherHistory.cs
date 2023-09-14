namespace WeatherBackendAfter.History.Models
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
                    ? EWeatherForecastType.Home : midTemperature / 2 > 0 && midTemperature / 2 <= 25
                    ? EWeatherForecastType.Walk : EWeatherForecastType.Swim;
            }
        }
    }
}
