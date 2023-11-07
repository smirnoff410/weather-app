using WeatherCommon.Services.Mapping;
using WeatherDatabase.Models;
using WeatherGrabber.Models;

namespace WeatherGrabber.Services.Mappings
{
    public class ForecastDatabaseToForecastGrabberModelMap : AbstractMapper<Forecast, ForecastGrabberModel>
    {
        public override ForecastGrabberModel Map(Forecast forecast)
        {
            return new ForecastGrabberModel
            {
                Temperature = forecast.Temperature.Value,
                Condition = forecast.Summary,
                Date = forecast.Date.Value
            };
        }
    }
}
