using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherTelegramService.Services.FollowCityFacade;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherTelegramService.Services.Telegram
{
    public class TelegramReceiverService : ITelegramReceiverService
    {
        private readonly ILogger<TelegramReceiverService> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceProvider _serviceProvider;

        public TelegramReceiverService(
            ITelegramBotClient botClient,
            IServiceProvider serviceProvider,
            ILogger<TelegramReceiverService> logger)
        {
            _logger = logger;
            _botClient = botClient;
            _serviceProvider = serviceProvider;
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

                if(messageText == "/city")
                {
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                        new KeyboardButton[] { "Volgograd 🗿" },
                        new KeyboardButton[] { "Moscow 🇷🇺" },
                        new KeyboardButton[] { "Saint Petersburg 🏰" }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Please enter city for subscribe forecast",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    return;
                }
                if(messageText == "/category")
                {
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                        new KeyboardButton[] { "Car 🚗" },
                        new KeyboardButton[] { "Sport 🏃🏻" },
                        new KeyboardButton[] { "Tourism 🗺️" }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Please enter category for subscribe recommendations",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                    return;
                }
                using var followCityService = new FollowCityFacadeService(_serviceProvider);
                await followCityService.Operation(chatId, messageText, $"{message.Chat.FirstName} {message.Chat.LastName}");
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
