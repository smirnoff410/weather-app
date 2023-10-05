using Microsoft.AspNetCore.Mvc;

namespace WeatherBackendAfter.WeatherForecast.Controllers
{
    using Microsoft.Data.SqlClient;
    using WeatherBackend.City.Repository;
    using WeatherBackend.Services.WeatherService;
    using WeatherBackend.WeatherForecast.Models;
    using WeatherBackendAfter.Services.WeatherService;
    using WeatherBackendAfter.WeatherForecast.Models;
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ICityRepository _cityRepository;
        public WeatherForecastController(ICityRepository cityRepository)
        {
            _weatherService = new WeatherService();
            _cityRepository = cityRepository;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<WeatherForecastFuture>> ListFuture()
        {
            var response = new List<WeatherForecastFuture>();
            foreach(var city in await _cityRepository.List())
            {
                var forecasts = _weatherService.GenerateHistory(DateTime.UtcNow, DateTime.UtcNow.AddDays(3)).ToList();
                foreach(var forecast in forecasts)
                {
                    response.Add(new WeatherForecastFuture { City = city, Date = forecast.Date, MaxTemperature = forecast.MaxTemperature, MinTemperature = forecast.MinTemperature });
                }
            }
            return response;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> Current()
        {
            return _weatherService.GenerateForCurrentDay(24).ToArray();
        }

        [HttpPost("[action]")]
        public async Task<Guid> CreateCurrent(CreateWeatherForecastDTO dto)
        {
            string connectionString = "Server=DESKTOP-LFM3LHR\\SQLEXPRESS;Database=WeatherDatabase;Trusted_Connection=True;Encrypt=false";
            var sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();
            var id = Guid.NewGuid();
            var insertCommand = new SqlCommand($"insert into [dbo].[WeatherForecasts] values ('{id}', {dto.Temperature}, '{dto.Summary}', '{DateTime.UtcNow}', '{dto.CityID}')", sqlConnection);

            await insertCommand.ExecuteNonQueryAsync();

            await sqlConnection.CloseAsync();

            return id;
        }
    }
}