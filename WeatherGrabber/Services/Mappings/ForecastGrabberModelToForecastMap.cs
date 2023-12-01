using WeatherCommon.Services.Mapping;
using WeatherDatabase.Models;
using WeatherGrabber.Models;

namespace WeatherGrabber.Services.Mappings
{
    public class ForecastGrabberModelToForecastMap : AbstractMapper<ForecastGrabberModel, Forecast>
    {
        public override Forecast Map(ForecastGrabberModel source)
        {
            return new Forecast
            {
                Summary = source.Condition,
                Date = source.Date,
                Temperature = source.Temperature
            };
        }
    }
}
