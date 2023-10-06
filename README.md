# Лабораторная работа №2
## CRUD сущности с помощью языка высокого уровня

#### Библиотека для C#
Для выполнения sql запросов на C# можно использовать библиотеку Microsoft.Data.SqlClient
Для этого достаточно в файле проекта с расширением .csproj прописать следующие строчки 
```csharp
<ItemGroup>
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.1" />
</ItemGroup>
```

Запросы к БД происходят в классе CityRepository, для которого определен следующий интерфейс
```csharp
public interface ICityRepository
{
    Task<IEnumerable<City>> List();
    Task<Guid> Create(CreateCityDTO dto);
    Task Update(Guid id, UpdateCityDTO dto);
    Task Delete(Guid id);
}
```

### Задание
Добавить взаимодействие с базой данных на языке высокого уровня и реализовать CRUD для сущности.

