using System.Text.Json;
using WeatherGrabber.DTO.WeatherAPI;

namespace WeatherGrabber.Clients
{
    public class WeatherApiClient : IWeatherClient
    {
        private readonly string _weatherApiKey = "f2681def0bc14b9da5d134125230610";

        private readonly HttpClient _httpClient;

        public WeatherApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("weatherapi");
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
