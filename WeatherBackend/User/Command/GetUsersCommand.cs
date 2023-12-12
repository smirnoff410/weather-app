using WeatherCommon.Services.Command;

namespace WeatherBackend.User.Command
{
    using Microsoft.EntityFrameworkCore;
    using WeatherBackend.User.Models;
    using WeatherDatabase.Models;
    using WeatherCommon.Models.Request;
    using WeatherDatabase.Repository;
    using WeatherCommon.Services.Mapping;

    public class GetUsersCommand : BaseHttpCommand<GetEntitiesRequest, ICollection<UserResponseItem>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMappingFactory _mappingFactory;

        public GetUsersCommand(IRepository<User> UserRepository, IMappingFactory mappingFactory, ILogger<GetUsersCommand> logger) : base(logger)
        {
            _userRepository = UserRepository;
            _mappingFactory = mappingFactory;
        }

        public override async Task<ICollection<UserResponseItem>> ExecuteResponse(GetEntitiesRequest request)
        {
            var users = await _userRepository.Get().Include(x => x.Cities).ToListAsync();

            var translator = _mappingFactory.GetMapper<User, UserResponseItem>();
            var result = users.Select(translator.Map).ToList();

            return result;
        }
    }
}
