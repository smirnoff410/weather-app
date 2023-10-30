using WeatherCommon.Services.Command;

namespace WeatherBackend.City.Command
{
    using Microsoft.EntityFrameworkCore;
    using WeatherBackend.City.Models;
    using WeatherDatabase.Models;
    using WeatherCommon.Models.Request;
    using WeatherDatabase.Repository;

    public class GetCitiesCommand : BaseHttpCommand<GetEntitiesRequest, ICollection<CityResponseItem>>
    {
        private readonly IRepository<City> _cityRepository;

        public GetCitiesCommand(IRepository<City> cityRepository, ILogger<GetCitiesCommand> logger) : base(logger)
        {
            _cityRepository = cityRepository;
        }

        public override async Task<ICollection<CityResponseItem>> ExecuteResponse(GetEntitiesRequest request)
        {
            var cities = await _cityRepository.Get().ToListAsync();

            var result = new List<CityResponseItem>();
            foreach (var city in cities) 
            {
                result.Add(new CityResponseItem
                {
                    ID = city.Id,
                    Name = city.Name
                });
            }

            return result;
        }
    }
}
