using WeatherDatabase.Specification;

namespace WeatherDatabase.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(Specification<T>? specification = null);
        Task Add(T entity);
        void Update(T entity);
        Task Delete(Guid id);
        void Delete(T entity);
    }
}
