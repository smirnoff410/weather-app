using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WeatherCommon.Services.Command
{
    public abstract class BaseHttpCommand<TRequest, TResponse> : ICommand<TRequest, TResponse>
    {
        protected readonly ILogger<ICommand<TRequest, TResponse>> _logger;

        public BaseHttpCommand(ILogger<ICommand<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public abstract Task<TResponse> ExecuteResponse(TRequest request);

        public async Task<HttpResponse> Execute(TRequest request)
        {
            try
            {
                _logger.LogInformation("Execute {0}, with request: {1}", request.GetType().Name, JsonSerializer.Serialize(request));

                var result = await ExecuteResponse(request);

                _logger.LogInformation("Execute {0} finished", request.GetType().Name);

                return new HttpResponse(result!);
            }
            catch (Exception ex)
            {
                _logger.LogError("Execute {0} finished with error {1}", request.GetType().Name, ex.Message);

                return new HttpResponse(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
