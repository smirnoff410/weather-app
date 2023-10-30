using WeatherBackend.City.Models;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using Microsoft.EntityFrameworkCore;
    using WeatherBackend.City.Specification;
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class CreateCityCommand : BaseHttpCommand<CreateEntityRequest<CreateCityDTO>, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;

        public CreateCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, ILogger<CreateCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
        }

        public override async Task<CityResponseItem> ExecuteResponse(CreateEntityRequest<CreateCityDTO> request)
        {
            var cityID = Guid.NewGuid();
            
            await _cityRepository.Add(new City
            {
                Id = cityID,
                Name = request.Dto.Name,
            });

            await _context.SaveChangesAsync();

            var newCity = await _cityRepository.Get(new GetCityByIdSpecification(cityID)).FirstOrDefaultAsync();
            return new CityResponseItem
            {
                ID = newCity.Id,
                Name = newCity.Name,
            };
        }
    }
}
