FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY "WeatherTelegramService/WeatherTelegramService.csproj" "WeatherTelegramService/WeatherTelegramService.csproj"
COPY "WeatherCommon/WeatherCommon.csproj" "WeatherCommon/WeatherCommon.csproj"
COPY "WeatherDatabase/WeatherDatabase.csproj" "WeatherDatabase/WeatherDatabase.csproj"

RUN dotnet restore "WeatherTelegramService/WeatherTelegramService.csproj"

COPY "WeatherTelegramService" "WeatherTelegramService"
COPY "WeatherCommon" "WeatherCommon"
COPY "WeatherDatabase" "WeatherDatabase"

RUN dotnet publish "WeatherTelegramService/WeatherTelegramService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "WeatherTelegramService.dll"]