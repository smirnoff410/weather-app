using WeatherCommon.Services.Command;

namespace WeatherBackend.City.Command
{
    using Microsoft.EntityFrameworkCore;
    using WeatherBackend.City.Models;
    using WeatherDatabase.Models;
    using WeatherCommon.Models.Request;
    using WeatherDatabase.Repository;
    using WeatherCommon.Services.Mapping;

    public class GetCitiesCommand : BaseHttpCommand<GetEntitiesRequest, ICollection<CityResponseItem>>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IMappingFactory _mapperFactory;

        public GetCitiesCommand(IRepository<City> cityRepository, IMappingFactory mapperFactory, ILogger<GetCitiesCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _mapperFactory = mapperFactory;
        }

        public override async Task<ICollection<CityResponseItem>> ExecuteResponse(GetEntitiesRequest request)
        {
            var cities = await _cityRepository.Get().ToListAsync();

            var translator = _mapperFactory.GetMapper<City, CityResponseItem>();
            return cities.Select(translator.Map).ToList();
        }
    }
}
