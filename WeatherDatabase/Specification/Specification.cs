using System.Linq.Expressions;

namespace WeatherDatabase.Specification
{
    public abstract class Specification<TEntity>
    {
        protected Specification(Expression<Func<TEntity, bool>>? expression) => Criteria = expression;

        public Expression<Func<TEntity, bool>>? Criteria { get; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();
        public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => IncludeExpressions.Add(includeExpression);

        protected void AddOrderBy(
            Expression<Func<TEntity, object>> orderByExporession) =>
            OrderByExpression = orderByExporession;
        protected void AddOrderByDescending(
            Expression<Func<TEntity, object>> orderByDescendingExporession) =>
            OrderByDescendingExpression = orderByDescendingExporession;
    }
}
