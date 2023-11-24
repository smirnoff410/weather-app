FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY "WeatherGrabber/WeatherGrabber.csproj" "WeatherGrabber/WeatherGrabber.csproj"
COPY "WeatherCommon/WeatherCommon.csproj" "WeatherCommon/WeatherCommon.csproj"
COPY "WeatherDatabase/WeatherDatabase.csproj" "WeatherDatabase/WeatherDatabase.csproj"

RUN dotnet restore "WeatherGrabber/WeatherGrabber.csproj"

COPY "WeatherGrabber" "WeatherGrabber"
COPY "WeatherCommon" "WeatherCommon"
COPY "WeatherDatabase" "WeatherDatabase"

RUN dotnet publish "WeatherGrabber/WeatherGrabber.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "WeatherGrabber.dll"]