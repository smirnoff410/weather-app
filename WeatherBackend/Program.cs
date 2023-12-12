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
using WeatherCommon.Services.Mapping;
using WeatherBackend.Services.Mappings;
using WeatherBackend.User.Models;
using WeatherBackend.User.Command;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddSingleton<IMappingFactory, WeatherBackendMappingFactory>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IMediator, CommandMediator>();

builder.Services.AddDbContext<WeatherDatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetSection("DatabaseSettings").GetSection("ConnectionString").Value));
builder.Services.AddScoped<IRepository<City>>(x => new Repository<City>(x.GetRequiredService<WeatherDatabaseContext>()));
builder.Services.AddScoped<IRepository<User>>(x => new Repository<User>(x.GetRequiredService<WeatherDatabaseContext>()));

#region City commands
builder.Services.AddScoped<ICommand<GetEntitiesRequest, ICollection<CityResponseItem>>, GetCitiesCommand>();
builder.Services.AddScoped<ICommand<CreateEntityRequest<CreateCityDTO>, CityResponseItem>, CreateCityCommand>();
builder.Services.AddScoped<ICommand<UpdateEntityRequest<UpdateCityDTO>, CityResponseItem>, UpdateCityCommand>();
builder.Services.AddScoped<ICommand<DeleteEntityRequest, CityResponseItem>, DeleteCityCommand>();
#endregion

#region User commands
builder.Services.AddScoped<ICommand<GetEntitiesRequest, ICollection<UserResponseItem>>, GetUsersCommand>();

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeatherDatabaseContext>();
    context.Database.Migrate();

    var translatorFactory = scope.ServiceProvider.GetRequiredService<IMappingFactory>();
    translatorFactory.Initialize();
}

app.Run();
