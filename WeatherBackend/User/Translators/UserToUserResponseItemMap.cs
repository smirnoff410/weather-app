using WeatherBackend.User.Models;
using WeatherCommon.Services.Mapping;

namespace WeatherBackend.Services.Mappings
{
    using WeatherBackend.City.Models;
    using WeatherDatabase.Models;
    public class UserToUserResponseItemMap : AbstractMapper<User, UserResponseItem>
    {
        private readonly IMappingFactory _mappingFactory;

        public UserToUserResponseItemMap(IMappingFactory mappingFactory)
        {
            _mappingFactory = mappingFactory;
        }
        public override UserResponseItem Map(User source)
        {
            var translator = _mappingFactory.GetMapper<City, CityResponseItem>();
            return new UserResponseItem 
            {
                ID = source.Id,
                Name = source.Name,
                Cities = source.Cities.Select(translator.Map).ToList()
            };
        }
    }
}
