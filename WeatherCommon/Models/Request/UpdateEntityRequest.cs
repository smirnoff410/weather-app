namespace WeatherCommon.Models.Request
{
    public record UpdateEntityRequest<T>(Guid Id, T Dto);
}
