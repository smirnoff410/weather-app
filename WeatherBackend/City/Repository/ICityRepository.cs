namespace WeatherBackend.City.Repository
{
    using WeatherBackend.City.Models;
    public interface ICityRepository
    {
        IEnumerable<City> List();
    }
}
