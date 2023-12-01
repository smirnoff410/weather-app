namespace WeatherCommon.Services.Mapping
{
    public interface IMapper
    {
        object Map(object entity);
    }
    public interface IMapper<TSource, TDestination> : IMapper
    {
        TDestination Map(TSource source);
    }
}
