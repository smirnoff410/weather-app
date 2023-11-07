namespace WeatherCommon.Services.Mapping
{
    public abstract class MappingFactory : IMappingFactory
    {
        protected List<IMapper> _mapperList = new ();
        
        public abstract void Initialize();

        public IMapper<TSource, TDestination> GetMapper<TSource, TDestination>() where TDestination : class where TSource : class
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var mapper = _mapperList.FirstOrDefault(x => x.GetType()
                .GetInterfaces()
                .Any(c => c.GenericTypeArguments.Contains(sourceType) && c.GenericTypeArguments.Contains(destinationType)));
            if (mapper == null)
                throw new NullReferenceException($"Mapper with source type {sourceType} and {destinationType}");
            if(mapper is not IMapper<TSource, TDestination>)
                throw new InvalidOperationException("Mapper not implement IMapper<TSource, TDestination> interface");

            return (mapper as IMapper<TSource, TDestination>)!;

        }
    }
}
