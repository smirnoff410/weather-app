using System;
using System.Collections.Generic;

namespace WeatherBackend.Models;

public partial class City
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
