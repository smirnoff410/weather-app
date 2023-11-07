using WeatherCommon.Services.Mapping;

namespace WeatherGrabber.Services.Mappings
{
    public class WeatherGrabberMappingFactory : MappingFactory, IWeatherGrabberMappingFactory
    {
        public override void Initialize()
        {
            _mapperList.Add(new ForecastDatabaseToForecastGrabberModelMap());
        }
    }
}
