using System;
using System.Collections.Generic;

namespace WeatherBackend.Models;

public partial class WeatherForecast
{
    public Guid Id { get; set; }

    public int? Temperature { get; set; }

    public string? Summary { get; set; }

    public DateTime? Date { get; set; }

    public Guid? CityId { get; set; }

    public virtual City? City { get; set; }
}
