using WeatherBackend.City.Translators;
using WeatherCommon.Services.Mapping;

namespace WeatherBackend.Services.Mappings
{
    public class WeatherBackendMappingFactory : MappingFactory, IMappingFactory
    {
        public override void Initialize()
        {
            _mapperList.Add(new UserToUserResponseItemMap(this));
            _mapperList.Add(new CreateCityDTOTOCityMap());
            _mapperList.Add(new CityToCityResponseItemMap());
        }
    }
}
