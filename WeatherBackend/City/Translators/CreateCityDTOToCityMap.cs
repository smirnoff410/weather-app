using WeatherBackend.City.Models;
using WeatherCommon.Services.Mapping;

namespace WeatherBackend.City.Translators
{
    using WeatherDatabase.Models;
    public class CreateCityDTOTOCityMap : AbstractMapper<CreateCityDTO, City>
    {
        public override City Map(CreateCityDTO source)
        {
            return new City 
            {
                Name = source.Name
            };
        }
    }
}