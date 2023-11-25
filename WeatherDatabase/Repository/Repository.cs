using Microsoft.EntityFrameworkCore;
using WeatherDatabase.Specification;

namespace WeatherDatabase.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            T? deleted = await _dbSet.FindAsync(id);
            if (deleted != null)
                _dbSet.Remove(deleted);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> Get(Specification<T>? specification = null)
        {
            if(specification != null)
                return SpecificationEvaluator.GetQuery(_dbSet, specification);

            return _dbSet;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
