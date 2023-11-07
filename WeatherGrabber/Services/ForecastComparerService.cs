using WeatherGrabber.DTO.WeatherAPI;

namespace WeatherGrabber.Services
{
    using WeatherDatabase.Models;
    public class ForecastComparerService
    {
        public ICollection<Forecast> Compare(ForecastDTO apiForecast)
        {
            var result = new List<Forecast>();

            foreach(var forecastDay in apiForecast.forecast.forecastday)
            {
                foreach(var forecastHour in forecastDay.hour)
                {
                    result.Add(new Forecast
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Parse(forecastHour.time),
                        Summary = forecastHour.condition.text,
                        Temperature = (int)forecastHour.temp_c
                    });
                }
            }

            return result;
        }
    }
}
