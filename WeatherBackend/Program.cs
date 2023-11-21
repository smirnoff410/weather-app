using Microsoft.EntityFrameworkCore;
using WeatherBackend.City.Command;
using WeatherBackend.City.Models;
using WeatherBackend.Services.WeatherService;
using WeatherBackend.Settings;
using WeatherDatabase.Models;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherCommon.Services.Mediator;
using WeatherDatabase.Repository;
using WeatherDatabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IMediator, CommandMediator>();

builder.Services.AddDbContext<WeatherDatabaseContext>();
builder.Services.AddScoped<IRepository<City>>(x => new Repository<City>(x.GetRequiredService<WeatherDatabaseContext>()));

builder.Services.AddScoped<ICommand<GetEntitiesRequest, ICollection<CityResponseItem>>, GetCitiesCommand>();
builder.Services.AddScoped<ICommand<CreateEntityRequest<CreateCityDTO>, CityResponseItem>, CreateCityCommand>();
builder.Services.AddScoped<ICommand<UpdateEntityRequest<UpdateCityDTO>, CityResponseItem>, UpdateCityCommand>();
builder.Services.AddScoped<ICommand<DeleteEntityRequest, CityResponseItem>, DeleteCityCommand>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
