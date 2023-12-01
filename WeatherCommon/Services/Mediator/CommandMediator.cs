using Microsoft.Extensions.DependencyInjection;
using WeatherCommon.Services.Command;

namespace WeatherCommon.Services.Mediator
{
    public class CommandMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<HttpResponse> Dispatch<TRequest, TResponse>(TRequest request)
        {
            Type type = typeof(ICommand<,>);
            Type[] typeArgs = { request.GetType(), typeof(TResponse) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            HttpResponse result = await handler.Execute((dynamic)request);

            return result;
        }
    }
}
