using System;

namespace WeatherDesktop
{
    public class CityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
