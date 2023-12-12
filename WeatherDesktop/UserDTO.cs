using System;
using System.Collections.Generic;

namespace WeatherDesktop
{
    public class UserDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public ICollection<CityDTO> Cities { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
