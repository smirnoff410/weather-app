using Microsoft.EntityFrameworkCore;
using WeatherBackend.City.Models;
using WeatherBackend.City.Specification;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Repository;

namespace WeatherBackend.City.Command
{
    using WeatherDatabase;
    using WeatherDatabase.Models;
    public class DeleteCityCommand : BaseHttpCommand<DeleteEntityRequest, CityResponseItem>
    {
        private readonly IRepository<City> _cityRepository;
        private readonly WeatherDatabaseContext _context;

        public DeleteCityCommand(IRepository<City> cityRepository, WeatherDatabaseContext context, ILogger<DeleteCityCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
            _context = context;
        }

        public override async Task<CityResponseItem> ExecuteResponse(DeleteEntityRequest request)
        {
            var city = await _cityRepository.Get(new GetCityByIdSpecification(request.ID)).FirstOrDefaultAsync();

            await _cityRepository.Delete(request.ID);

            await _context.SaveChangesAsync();

            return new CityResponseItem
            {
                ID = city.Id,
                Name = city.Name
            };
        }
    }
}
