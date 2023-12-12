using WeatherBackend.City.Models;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using Microsoft.EntityFrameworkCore;
    using WeatherBackend.City.Specification;
    using WeatherCommon.Services.Mapping;
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class CreateCityCommand : BaseHttpCommand<CreateEntityRequest<CreateCityDTO>, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;
        private readonly IMappingFactory _mappingFactory;

        public CreateCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, IMappingFactory mappingFactory, ILogger<CreateCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
            _mappingFactory = mappingFactory;
        }

        public override async Task<CityResponseItem> ExecuteResponse(CreateEntityRequest<CreateCityDTO> request)
        {
            var cityDtoToCityTranslator = _mappingFactory.GetMapper<CreateCityDTO, City>();

            var city = cityDtoToCityTranslator.Map(request.Dto);
            city.Id = Guid.NewGuid();

            await _cityRepository.Add(city);

            await _context.SaveChangesAsync();

            var newCity = await _cityRepository.Get(new GetCityByIdSpecification(city.Id)).FirstAsync();
            var cityToCityResponseTransaltor = _mappingFactory.GetMapper<City, CityResponseItem>();
            return cityToCityResponseTransaltor.Map(newCity);
        }
    }
}
