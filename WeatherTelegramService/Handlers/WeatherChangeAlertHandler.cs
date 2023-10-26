using Telegram.Bot;
using WeatherCommon.Services.Handlers;

namespace WeatherTelegramService.Handlers
{
    public class WeatherChangeAlertHandler : IBaseHandler<string>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<WeatherChangeAlertHandler> _logger;

        public WeatherChangeAlertHandler(ITelegramBotClient botClient, ILogger<WeatherChangeAlertHandler> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }
        public async Task Handle(string request)
        {
            Console.WriteLine(request);

            await _botClient.SendTextMessageAsync(
                chatId: 334648141,
                text: request);
        }
    }
}
