using Microsoft.EntityFrameworkCore;

namespace WeatherBackend.Models;

public partial class WeatherDatabaseContext : DbContext
{

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Forecast> Forecasts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlServer("Server=DESKTOP-BOLD0JC\\SQLEXPRESS;Database=WeatherDatabaseTemp;Trusted_Connection=True;Encrypt=false");
}
