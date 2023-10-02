namespace WeatherBackend.City.Repository
{
    using Models;
    public class CityRepository : ICityRepository
    {
        public IEnumerable<City> List()
        {
            return new List<City>()
            {
                new City { ID = Guid.NewGuid(), Name = "Volgograd" },
                new City { ID = Guid.NewGuid(), Name = "Moscow" },
                new City { ID = Guid.NewGuid(), Name = "Saint-Petersburg" },
                new City { ID = Guid.NewGuid(), Name = "Ekaterinburg" },
                new City { ID = Guid.NewGuid(), Name = "Vladivostok" },
                new City { ID = Guid.NewGuid(), Name = "Kaliningrad" }
            };
        }
    }
}
