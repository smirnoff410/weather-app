using Microsoft.EntityFrameworkCore;
using WeatherDatabase.Models;

namespace WeatherDatabase;

public partial class WeatherDatabaseContext : DbContext
{

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Forecast> Forecasts { get; set; }

    public WeatherDatabaseContext()
    {

    }
    public WeatherDatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlServer("Server=DESKTOP-BOLD0JC\\SQLEXPRESS;Database=WeatherDatabaseTemp;Trusted_Connection=True;Encrypt=false");
}
