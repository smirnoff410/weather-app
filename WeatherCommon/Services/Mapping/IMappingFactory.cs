namespace WeatherCommon.Services.Mapping
{
    public interface IMappingFactory
    {
        void Initialize();

        IMapper<TSource, TDestination> GetMapper<TSource, TDestination>() where TDestination : class where TSource : class;
    }
}
