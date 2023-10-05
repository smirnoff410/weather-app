using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.City.Controllers
{
    using WeatherBackend.City.Models;
    using WeatherBackend.City.Repository;

    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        [HttpGet("")]
        public async Task<IEnumerable<City>> GetCities()
        {
            return await _cityRepository.List();
        }

        [HttpPost("[action]")]
        public async Task<Guid> Create(CreateCityDTO dto)
        {
            return await _cityRepository.Create(dto);
        }
    }
}
