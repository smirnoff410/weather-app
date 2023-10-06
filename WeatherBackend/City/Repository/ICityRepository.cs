namespace WeatherBackend.City.Repository
{
    using WeatherBackend.City.Models;
    public interface ICityRepository
    {
        Task<IEnumerable<City>> List();
        Task<Guid> Create(CreateCityDTO dto);
        Task Update(Guid id, UpdateCityDTO dto);
        Task Delete(Guid id);
    }
}
