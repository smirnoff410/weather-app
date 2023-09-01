using System;

namespace WeatherDesktop
{
    public class WeatherDTO
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string? Summary { get; set; }

        public override string ToString()
        {
            return $"{Date}. TemperatureC: {TemperatureC}. TemperatureF: {TemperatureF}. Summary: {Summary}";
        }
    }
}
