using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherDatabase.Repository;
using WeatherDatabase.Models;
using Microsoft.EntityFrameworkCore;
using WeatherDatabase;

namespace WeatherTelegramService.Services.Telegram
{
    public class TelegramReceiverService : ITelegramReceiverService
    {
        private readonly ILogger<TelegramReceiverService> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly IRepository<WeatherDatabase.Models.User> userRepository;
        private readonly IRepository<City> cityRepository;
        private readonly WeatherDatabaseContext context;

        public TelegramReceiverService(
            ITelegramBotClient botClient,
            IRepository<WeatherDatabase.Models.User> userRepository,
            IRepository<City> cityRepository,
            WeatherDatabaseContext context,
            ILogger<TelegramReceiverService> logger)
        {
            _logger = logger;
            _botClient = botClient;
            this.userRepository = userRepository;
            this.cityRepository = cityRepository;
            this.context = context;
        }
        public void Initialize()
        {
            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            /*var userRepository = _serviceScope.ServiceProvider.GetRequiredService<IRepository<WeatherDatabase.Models.User>>();
            var cityRepository = _serviceScope.ServiceProvider.GetRequiredService<IRepository<City>>();
            var context = _serviceScope.ServiceProvider.GetRequiredService<WeatherDatabaseContext>();*/
            try
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;
                _logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

                if (messageText == "/start")
                {
                    // Echo received message text
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Please enter city for subscribe forecast",
                        cancellationToken: cancellationToken);
                    return;
                }

                var user = await userRepository.Get().FirstOrDefaultAsync(x => x.ChatId == chatId);
                user ??= new WeatherDatabase.Models.User { Id = Guid.NewGuid(), ChatId = chatId, Name = $"{message.Chat.FirstName} {message.Chat.LastName}" };

                var city = await cityRepository.Get().FirstOrDefaultAsync(x => x.Name == messageText);
                city ??= new City { Id = Guid.NewGuid(), Name = messageText };
                await cityRepository.Add(city);
                await userRepository.Add(user);

                await context.SaveChangesAsync();
            }
            catch
            {

            }
        }
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(errorMessage);
            return Task.CompletedTask;
        }
    }
}
