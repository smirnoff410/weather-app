using Microsoft.EntityFrameworkCore;

namespace WeatherBackend.Models;

public partial class WeatherDatabaseContext : DbContext
{
    public WeatherDatabaseContext()
    {
    }

    public WeatherDatabaseContext(DbContextOptions<WeatherDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC27C13C9B0D");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Users).WithMany(p => p.Cities)
                .UsingEntity<Dictionary<string, object>>(
                    "CityUser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    l => l.HasOne<City>().WithMany().HasForeignKey("CityId"),
                    j =>
                    {
                        j.HasKey("CityId", "UserId");
                        j.ToTable("CityUsers");
                        j.IndexerProperty<Guid>("CityId").HasColumnName("CityID");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeatherF__3214EC27A5302E8B");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Summary).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.WeatherForecasts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__WeatherFo__CityI__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
