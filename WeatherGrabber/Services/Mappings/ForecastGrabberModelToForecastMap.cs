using WeatherCommon.Services.Mapping;
using WeatherDatabase.Models;
using WeatherGrabber.Models;

namespace WeatherGrabber.Services.Mappings
{
    public class ForecastGrabberModelToForecastMap : IMapper<ForecastGrabberModel, Forecast>
    {

        public object Map(object entity)
        {
            throw new NotImplementedException();
        }

        public Forecast Map(ForecastGrabberModel source)
        {
            return new Forecast
            {
                Summary = source.Condition,
                Date = source.Date,
                Temperature = Convert.ToInt32(source.Temperature)
            };
        }
    }
}
