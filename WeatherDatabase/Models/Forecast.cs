using System;
using System.Collections.Generic;

namespace WeatherDatabase.Models;

public partial class Forecast
{
    public Guid Id { get; set; }

    public int? Temperature { get; set; }

    public string? Summary { get; set; }

    public DateTime? Date { get; set; }

    public Guid? CityId { get; set; }

    public virtual City? City { get; set; }
}
