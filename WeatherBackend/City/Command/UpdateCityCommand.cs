using Microsoft.EntityFrameworkCore;
using WeatherBackend.City.Models;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using WeatherBackend.City.Specification;
    using WeatherCommon.Services.Mapping;
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class UpdateCityCommand : BaseHttpCommand<UpdateEntityRequest<UpdateCityDTO>, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;
        private readonly IMappingFactory _mapperFactory;

        public UpdateCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, IMappingFactory mapperFactory, ILogger<UpdateCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
            _mapperFactory = mapperFactory;
        }

        public override async Task<CityResponseItem> ExecuteResponse(UpdateEntityRequest<UpdateCityDTO> request)
        {
            var city = await _cityRepository.Get(new GetCityByIdSpecification(request.Id)).FirstAsync();

            city.Name = request.Dto.Name;

            _cityRepository.Update(city);

            await _context.SaveChangesAsync();

            var translator = _mapperFactory.GetMapper<City, CityResponseItem>();
            var updatedCity = await _cityRepository.Get(new GetCityByIdSpecification(request.Id)).FirstAsync();
            return translator.Map(updatedCity);
        }
    }
}
