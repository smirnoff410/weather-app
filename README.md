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

### Задание
Для своего проекта выполнить 2 подхода. Если уже существует база данных, то легче сначала выполнить подход Database First, а потом на основе автоматически созданных классов создать новую базу данных с помощью подхода Code First
