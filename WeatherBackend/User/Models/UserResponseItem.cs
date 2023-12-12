using WeatherBackend.City.Models;

namespace WeatherBackend.User.Models
{
    public class UserResponseItem
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<CityResponseItem> Cities { get; set; } = new List<CityResponseItem>();
    }
}
