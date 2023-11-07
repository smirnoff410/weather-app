# Лабораторная работа №5
## Git pull request

Паттерны GoF(Gang of four) подразделяются на 3 категории: порождающие, структурные и поведенческие.

### Задание
Реализовать любой паттерн из каждой категории(всего 3 паттерна и они не должны часто повторяться у студентов).

### Порождающие
Отвечают за удобное и безопасное создание новых объектов или даже целых семейств объектов.
В проекте реализован паттерн Строитель. [WeatherTelegramService.Services.ServiceBuilder](https://github.com/smirnoff410/weather-app/blob/oop/Lab3/WeatherTelegramService/Services/ServiceBuilder/ServiceProviderBuilder.cs)

Использование в проекте

```csharp
ServiceProviderBuilder spBuilder = new(services);
spBuilder
    .AddTelegramReceiverService()
    .AddMessageQueue()
    .Build();
```

### Структурные
Отвечают за построение удобных в поддержке иерархий классов.
В проекте реализован паттер Фасад. [WeatherTelegramService.Services.FollowCityFacade](https://github.com/smirnoff410/weather-app/blob/oop/Lab3/WeatherTelegramService/Services/FollowCityFacade/FollowCityFacadeService.cs)

Использование в проекте

```csharp
using var followCityService = new FollowCityFacadeService(_serviceProvider);
await followCityService.Operation(chatId, messageText, $"{message.Chat.FirstName} {message.Chat.LastName}");
```

### Поведенческие
Решают задачи эффективного и безопасного взаимодействия между объектами программы.
В проекте реализован паттерн Команда. [WeatherCommon.Services.Command](https://github.com/smirnoff410/weather-app/blob/oop/Lab3/WeatherCommon/Services/Command/ICommand.cs)

Использование в проекте
```csharp
public class WeatherChangeAlertCommand : BaseCommand<WeatherChangeAlertRequest>
{
    private readonly ITelegramBotClient _botClient;

    public WeatherChangeAlertCommand(ITelegramBotClient botClient, ILogger<WeatherChangeAlertCommand> logger) : base(logger)
    {
        _botClient = botClient;
    }

    public override async Task ExecuteCommand(WeatherChangeAlertRequest request)
    {
        await _botClient.SendTextMessageAsync(
            chatId: request.ChatID,
            text: request.Text);
    }
}
```

### Полезные ссылки
https://refactoring.guru/ru/design-patterns/catalog - доступно с VPN

https://metanit.com/sharp/patterns/
