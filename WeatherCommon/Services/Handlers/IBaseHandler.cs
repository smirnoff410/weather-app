namespace WeatherCommon.Services.Handlers
{
    public interface IBaseHandler<TRequest>
    {
        Task Handle(TRequest request);
    }
}
