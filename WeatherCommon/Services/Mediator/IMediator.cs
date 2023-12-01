using WeatherCommon.Services.Command;

namespace WeatherCommon.Services.Mediator
{
    public interface IMediator
    {
        Task<HttpResponse> Dispatch<TRequest, TResponse>(TRequest request);
    }
}
