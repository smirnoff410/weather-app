using Telegram.Bot;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;

namespace WeatherTelegramService.Handlers
{
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
}
