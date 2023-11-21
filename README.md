# Лабораторная работа №6
## Docker

### Задание
1. Создать docker образы для каждого сервиса
2. Создать контейнеры на основе docker образов
3. Обернуть все Dockerfile в docker-compose

### Шаги
1. Создайте текстовый файл с названием Dockerfile для сервиса, например `weather-app/WeatherBackend/Dockerfile`

2. Наполните файл инструкциями докера, например
```
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY "WeatherBackend/WeatherBackend.csproj" "WeatherBackend/WeatherBackend.csproj"
COPY "WeatherCommon/WeatherCommon.csproj" "WeatherCommon/WeatherCommon.csproj"
COPY "WeatherDatabase/WeatherDatabase.csproj" "WeatherDatabase/WeatherDatabase.csproj"

RUN dotnet restore "WeatherBackend/WeatherBackend.csproj"

COPY "WeatherBackend" "WeatherBackend"
COPY "WeatherCommon" "WeatherCommon"
COPY "WeatherDatabase" "WeatherDatabase"

RUN dotnet publish "WeatherBackend/WeatherBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "WeatherBackend.dll"]
```
ПРИМЕЧАНИЕ: При создании нового проекта в Visual Studio предлагается автоматически сгенерировать Dockerfile. Также шаблоны Dockerfile можно найти для своего языка в интернете.

3. Создайте образ сервиса, для этого нужно открыть консоль на уровне weather-app: `docker build -t weather_backend_image -f WeatherBackend/Dockerfile .`

4. Убедиться в создании образа можно с помощью команды `docker image ls`

5. Запустите контейнер на основе образа с помощью команды `docker run -p 8000:80 --name weather_backend weather_backend_image`

Чтобы запустить контейнер в отдельном потоке, неободимо при запуске контейнера передать параметр `-d` 

6. Убедиться в создании контейнера можно с помощью команды `docker ps -a`

ПРИМЕЧАНИЕ: Для очистки памяти можно удалять ненужные образы и контейнеры с помощью команд:

- образы - docker image rm <image_id>
- контейнеры - docker rm <container_id>

7. Создайте в корне проекта файл docker-compose.yml, например со следующим содержимым
```
version: "3.9"
services:
  backend:
    hostname: weather_backend
    container_name: weather_backend
    build:
      context: .
      dockerfile: WeatherBackend/Dockerfile
    ports:
      - "8000:80"
```

8. Создайте образы и контейнеры по инструкциям в файле docker compose с помощью команды: `docker-compose up -d`

9. Создайте Dockerfile для остальных своих сервисов и внесите их в файл docker-compose
