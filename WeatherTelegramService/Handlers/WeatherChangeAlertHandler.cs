using Telegram.Bot;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;

namespace WeatherTelegramService.Handlers
{
    public class WeatherChangeAlertHandler : BaseCommand<WeatherChangeAlertRequest>
    {
        private readonly ITelegramBotClient _botClient;

        public WeatherChangeAlertHandler(ITelegramBotClient botClient, ILogger logger) : base(logger)
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
}
