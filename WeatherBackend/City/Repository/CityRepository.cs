namespace WeatherBackend.City.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Threading.Tasks;
    using WeatherBackend.Models;
    public class CityRepository : ICityRepository
    {
        private readonly WeatherDatabaseContext _context;

        public CityRepository(WeatherDatabaseContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(CreateCityDTO dto)
        {
            var newCity = new City
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await _context.Cities.AddAsync(newCity);

            await _context.SaveChangesAsync();

            return newCity.Id;
        }

        public async Task Delete(Guid id)
        {
            var deleteCity = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteCity != null)
            {
                _context.Cities.Remove(deleteCity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<City>> List()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task Update(Guid id, UpdateCityDTO dto)
        {
            var updateCity = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if(updateCity != null)
            {
                updateCity.Name = dto.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
