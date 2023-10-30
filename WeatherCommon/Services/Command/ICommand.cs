namespace WeatherCommon.Services.Command
{
    public interface ICommand<TRequest>
    {
        Task Execute(TRequest request);
    }
    public interface ICommand<TRequest, TResponse>
    {
        Task<HttpResponse> Execute(TRequest request);
    }
}
