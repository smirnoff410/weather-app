using Microsoft.EntityFrameworkCore;
using WeatherBackend.City.Models;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using WeatherBackend.City.Specification;
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class UpdateCityCommand : BaseHttpCommand<UpdateEntityRequest<UpdateCityDTO>, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;

        public UpdateCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, ILogger<UpdateCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
        }

        public override async Task<CityResponseItem> ExecuteResponse(UpdateEntityRequest<UpdateCityDTO> request)
        {
            var city = await _cityRepository.Get(new GetCityByIdSpecification(request.Id)).FirstOrDefaultAsync();

            city.Name = request.Dto.Name;

            _cityRepository.Update(city);

            await _context.SaveChangesAsync();

            var newCity = await _cityRepository.Get(new GetCityByIdSpecification(request.Id)).FirstOrDefaultAsync();
            return new CityResponseItem
            {
                ID = newCity.Id,
                Name = newCity.Name,
            };
        }
    }
}
