using System;
using System.Collections.Generic;

namespace WeatherDatabase.Models;

public partial class City
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Forecast> WeatherForecasts { get; set; } = new List<Forecast>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
