﻿namespace WeatherBackendAfter.Services.WeatherService
{
    using WeatherBackendAfter.History.Models;
    using WeatherBackendAfter.WeatherForecast.Models;
    public class WeatherService
    {
        private readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> GenerateForCurrentDay(int toHours)
        {
            return Enumerable.Range(1, toHours).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddHours(index),
                Temperature = Random.Shared.Next(-20, 45),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }

        public IEnumerable<WeatherHistory> GenerateHistory(DateTime fromDate, DateTime toDate)
        {
            var days = toDate - fromDate;
            return Enumerable.Range(1, days.Days).Select(index => new WeatherHistory
            {
                Date = DateOnly.FromDateTime(fromDate.AddDays(index)),
                MinTemperature = Random.Shared.Next(-20, 15),
                MaxTemperature = Random.Shared.Next(15, 45)
            });
        }
    }
}
