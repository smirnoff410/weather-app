using WeatherBackend.City.Models;
using WeatherCommon.Services.Mapping;

namespace WeatherBackend.City.Translators
{
    using WeatherDatabase.Models;
    public class CityToCityResponseItemMap : AbstractMapper<City, CityResponseItem>
    {
        public override CityResponseItem Map(City source)
        {
            return new CityResponseItem 
            {
                ID = source.Id,
                Name = source.Name
            };
        }
    }
}