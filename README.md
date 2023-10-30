# Лабораторная работа №3
## Database First, Code First

### Database First
Database First - подход для управления контекстом Entity Framework на основе существующей базы данных

Для использования необходимо: 
- С помощью консоли диспетчера пакетов установить инструмент Install-Package Microsoft.EntityFrameworkCore.Tools
- Добавить Nuget пакет Microsoft.EntityFrameworkCore.Design
- Добавить Nuget пакет поставщика баз данных (SqlServer, PostgreSQL и т.д.)

С помощью консоли диспетчера пакетов в Visual Studio выполнить реконструкцию контекста, например следующей командой:
Scaffold-DbContext "Server=DESKTOP-LFM3LHR\SQLEXPRESS;Database=WeatherDatabase;Trusted_Connection=True;Encrypt=false" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f

Подробнее о Scaffold: https://learn.microsoft.com/ru-ru/ef/core/managing-schemas/scaffolding/?tabs=vs

### Code First
Code First - подход для управления базой данных на основе контекста EntityFramework

Для использования необходимо:
- С помощью консоли диспетчера пакетов установить инструмент Install-Package Microsoft.EntityFrameworkCore.Tools и используемый провайдер, например Install-Package Microsoft.EntityFrameworkCore.SqlServer
- В контексте EntityFramework оставить только DbSet и метод OnConfiguring
```csharp
public partial class WeatherDatabaseContext : DbContext
{
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Forecast> Forecasts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlServer("Server=DESKTOP-BOLD0JC\\SQLEXPRESS;Database=WeatherDatabaseTemp;Trusted_Connection=True;Encrypt=false");
}
```
- Чтобы сгенерировать миграцию, в консоли диспетчера пакетов выполнить: Add-Migration InitialCreate
- Для обновления базы данных все в той же консоли: Update-Database

Между миграциями можно переключаться, тем самым понижать или повышать версию БД.
Update-Database <migration_name>

Подробнее о Code First: https://learn.microsoft.com/ru-ru/ef/core/get-started/overview/first-app?tabs=visual-studio

### Задание
Для своего проекта выполнить 2 подхода. Если уже существует база данных, то легче сначала выполнить подход Database First, а потом на основе автоматически созданных классов создать новую базу данных с помощью подхода Code First
