using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.WeatherBackend>("backendservice");

builder.AddProject<Projects.WeatherGrabber>("grabberservice");
builder.AddProject<Projects.WeatherTelegramService>("telegramservice");

builder.Build().Run();
