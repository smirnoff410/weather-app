using Microsoft.AspNetCore.Mvc;

namespace WeatherBackend.WeatherForecast.Controllers
{
    using Microsoft.Data.SqlClient;
    using WeatherDatabase.Repository;
    using WeatherBackend.Services.WeatherService;
    using WeatherBackend.WeatherForecast.Models;
    using WeatherDatabase.Models;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IRepository<City> _cityRepository;
        public WeatherForecastController(IRepository<City> cityRepository)
        {
            _weatherService = new WeatherService();
            _cityRepository = cityRepository;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<WeatherForecastFuture>> ListFuture()
        {
            var response = new List<WeatherForecastFuture>();
            foreach(var city in await _cityRepository.Get().ToListAsync())
            {
                var forecasts = _weatherService.GenerateHistory(DateTime.UtcNow, DateTime.UtcNow.AddDays(3)).ToList();
                foreach(var forecast in forecasts)
                {
                    response.Add(new WeatherForecastFuture { City = city.Name, Date = forecast.Date, MaxTemperature = forecast.MaxTemperature, MinTemperature = forecast.MinTemperature });
                }
            }
            return response;
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