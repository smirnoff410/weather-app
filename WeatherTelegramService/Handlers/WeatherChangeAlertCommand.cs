using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using WeatherCommon.Models.Request;
using WeatherCommon.Services.Command;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase.Specification.User;

namespace WeatherTelegramService.Handlers
{
    public class WeatherChangeAlertCommand : BaseCommand<WeatherChangeAlertRequest>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IRepository<User> _userRepository;

        public WeatherChangeAlertCommand(ITelegramBotClient botClient, IRepository<User> userRepository, ILogger<WeatherChangeAlertCommand> logger) : base(logger)
        {
            _botClient = botClient;
            _userRepository = userRepository;
        }

        public override async Task ExecuteCommand(WeatherChangeAlertRequest request)
        {
            var users = await _userRepository.Get(new GetUsersByCityIDSpecification(request.CityID)).ToListAsync();
            foreach(var user in users)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: user.ChatId,
                    text: request.Text);
            }
        }
    }
}
