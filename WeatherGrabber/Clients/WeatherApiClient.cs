using System.Text.Json;
using WeatherGrabber.DTO.WeatherAPI;

namespace WeatherGrabber.Clients
{
    public class WeatherApiClient : IWeatherClient
    {
        private readonly string _weatherApiKey;

        private readonly HttpClient _httpClient;

        public WeatherApiClient(IHttpClientFactory httpClientFactory, string weatherApiKey)
        {
            _httpClient = httpClientFactory.CreateClient("weatherapi");
            _weatherApiKey = weatherApiKey;
        }

        public async Task<ForecastDTO> GetForecast(string city)
        {
            var response = await _httpClient.GetAsync($"forecast.json?q={city}&days=1&key={_weatherApiKey}");
            if(response.StatusCode is not System.Net.HttpStatusCode.OK)
            {
                throw new Exception("forecast.json not OK response");
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var parsedResult = JsonSerializer.Deserialize<ForecastDTO>(jsonResult);
            
            return parsedResult!;
        }
    }

    public interface IWeatherClient
    {
        Task<ForecastDTO> GetForecast(string city);
    }
}
