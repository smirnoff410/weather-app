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

        [HttpPut("[action]/{id:guid}")]
        public async Task Update(Guid id, UpdateCityDTO dto)
        {
            await _cityRepository.Update(id, dto);
        }

        [HttpDelete("[action]/{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _cityRepository.Delete(id);
        }
    }
}
