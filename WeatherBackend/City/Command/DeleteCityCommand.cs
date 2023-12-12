using Microsoft.EntityFrameworkCore;
using WeatherBackend.City.Models;
using WeatherBackend.City.Specification;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using WeatherCommon.Services.Mapping;
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class DeleteCityCommand : BaseHttpCommand<DeleteEntityRequest, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;
        private readonly IMappingFactory _mappingFactory;

        public DeleteCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, IMappingFactory mappingFactory, ILogger<DeleteCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
            _mappingFactory = mappingFactory;
        }

        public override async Task<CityResponseItem> ExecuteResponse(DeleteEntityRequest request)
        {
            var city = await _cityRepository.Get(new GetCityByIdSpecification(request.ID)).FirstAsync();

            await _cityRepository.Delete(request.ID);

            await _context.SaveChangesAsync();
            
            var cityToCityResponseTransaltor = _mappingFactory.GetMapper<City, CityResponseItem>();
            return cityToCityResponseTransaltor.Map(city);
        }
    }
}
