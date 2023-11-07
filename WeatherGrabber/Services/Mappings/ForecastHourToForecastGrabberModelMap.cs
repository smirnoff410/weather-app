using WeatherCommon.Services.Mapping;
using WeatherGrabber.DTO.WeatherAPI;
using WeatherGrabber.Models;

namespace WeatherGrabber.Services.Mappings
{
    public class ForecastHourToForecastGrabberModelMap : AbstractMapper<Hour, ForecastGrabberModel>
    {
        public override ForecastGrabberModel Map(Hour forecast)
        {
            return new ForecastGrabberModel
            {
                Condition = forecast.condition.text,
                Date = DateTime.Parse(forecast.time),
                Temperature = forecast.temp_c
            };
        }
    }
}
