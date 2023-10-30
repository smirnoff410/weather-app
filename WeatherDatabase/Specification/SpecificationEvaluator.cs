using Microsoft.EntityFrameworkCore;

namespace WeatherDatabase.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> inputQueryable,
            Specification<TEntity> specification) where TEntity : class
        {
            var querable = inputQueryable;

            if (specification.Criteria is not null)
                querable = querable.Where(specification.Criteria);

            specification.IncludeExpressions.Aggregate(
                querable,
                (current, IncludeExpression) => current.Include(IncludeExpression));

            if(specification.OrderByExpression is not null)
                querable = querable.OrderBy(specification.OrderByExpression);
            else if(specification.OrderByDescendingExpression is not null)
                querable = querable.OrderByDescending(specification.OrderByDescendingExpression);

            return querable;
        }
    }
}
