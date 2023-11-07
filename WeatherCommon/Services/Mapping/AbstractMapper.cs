namespace WeatherCommon.Services.Mapping
{
    public abstract class AbstractMapper<T, D> : IMapper<T, D>
    {
        public abstract D Map(T source);

        public virtual object Map(object entity)
        {
            return Map(entity);
        }
    }
}
