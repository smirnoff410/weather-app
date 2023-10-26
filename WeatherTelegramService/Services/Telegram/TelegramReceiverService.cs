using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WeatherTelegramService.Services.Telegram
{
    public class TelegramReceiverService : ITelegramReceiverService
    {
        private readonly ILogger<TelegramReceiverService> _logger;
        private readonly ITelegramBotClient _botClient;

        public TelegramReceiverService(ITelegramBotClient botClient, ILogger<TelegramReceiverService> logger)
        {
            _logger = logger;
            _botClient = botClient;
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
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            /*
             Add new user to database with chatID and city
             */

            _logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

            if (messageText == "/start")
            {
                // Echo received message text
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Please enter city for subscribe forecast",
                    cancellationToken: cancellationToken);
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
