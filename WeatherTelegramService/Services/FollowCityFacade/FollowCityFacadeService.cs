using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDatabase.Models;
using WeatherDatabase.Repository;
using WeatherDatabase;
using Microsoft.EntityFrameworkCore;

namespace WeatherTelegramService.Services.FollowCityFacade
{
    public class FollowCityFacadeService : IDisposable
    {
        private readonly IServiceScope _serviceScope;

        public FollowCityFacadeService(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
        }

        public async Task Operation(long chatId, string messageText, string userName)
        {
            var userRepository = _serviceScope.ServiceProvider.GetRequiredService<IRepository<User>>();
            var cityRepository = _serviceScope.ServiceProvider.GetRequiredService<IRepository<City>>();
            var context = _serviceScope.ServiceProvider.GetRequiredService<WeatherDatabaseContext>();
            var user = await userRepository.Get().FirstOrDefaultAsync(x => x.ChatId == chatId);
            if (user == null)
            {
                user = new User { Id = Guid.NewGuid(), ChatId = chatId, Name = userName };
                await userRepository.Add(user);
            }

            var city = await cityRepository.Get().FirstOrDefaultAsync(x => x.Name == messageText);
            if (city == null)
            {
                city = new City { Id = Guid.NewGuid(), Name = messageText };
                await cityRepository.Add(city);
            }

            city.Users.Add(user);

            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}
