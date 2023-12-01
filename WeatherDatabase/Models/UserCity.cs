using System;
using System.Collections.Generic;

namespace WeatherDatabase.Models;

public partial class UserCity
{
    public Guid UserId { get; set; }

    public Guid CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
