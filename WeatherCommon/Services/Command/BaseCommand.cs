using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WeatherCommon.Services.Command
{
    public abstract class BaseCommand<TRequest> : ICommand<TRequest>
    {
        private readonly ILogger _logger;

        public BaseCommand(ILogger logger)
        {
            _logger = logger;
        }

        public abstract Task ExecuteCommand(TRequest request);

        public async Task Execute(TRequest request)
        {
            try
            {
                _logger.LogInformation("Execute {0}, with request: {1}", request.GetType().Name, JsonSerializer.Serialize(request));

                await ExecuteCommand(request);

                _logger.LogInformation("Execute {0} finished", request.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError("Execute {0} finished with error {1}", request.GetType().Name, ex.Message);
            }
        }
    }
}
