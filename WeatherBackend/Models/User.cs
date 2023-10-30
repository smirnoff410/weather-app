using System;
using System.Collections.Generic;

namespace WeatherBackend.Models;

public partial class User
{
    public Guid Id { get; set; }

    public long ChatId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
