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
        public IEnumerable<City> GetCities()
        {
            return _cityRepository.List();
        }
    }
}
