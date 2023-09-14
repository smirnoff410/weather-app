using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.City.Controllers
{
    public class CityController : ControllerBase
    {
        private readonly Dictionary<Guid, string> _cities = new()
        {
            { Guid.NewGuid(), "Volgograd" },
            { Guid.NewGuid(), "Moscow" },
            { Guid.NewGuid(), "Saint-Petersburg" },
            { Guid.NewGuid(), "Ekaterinburg" },
            { Guid.NewGuid(), "Vladivostok" },
            { Guid.NewGuid(), "Kaliningrad" }
        };

        [HttpGet("")]
        public Dictionary<Guid, string> GetCities()
        {
            return _cities;
        }
    }
}
