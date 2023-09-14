using System;

namespace WeatherDesktop
{
    public class WeatherDTO
    {
        public DateTime Date { get; set; }
        public int Temperature { get; set; }
        public string? Summary { get; set; }

        public override string ToString()
        {
            return $"{Date}. TemperatureC: {Temperature}. Summary: {Summary}";
        }
    }
}
