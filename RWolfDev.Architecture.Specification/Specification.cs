using RWolfDev.Architecture.Specification.Extensions;

using System.Linq.Expressions;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Base class for defining query specifications that can be applied to IQueryable collections.
    /// Supports criteria (filters), includes, ordering, and pagination.
    /// </summary>
    /// <typeparam name="T">The entity type to which the specification applies.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Backing field for the aggregated filter criteria expression.
        /// </summary>
        private Expression<Func<T, bool>>? _criteria;

        /// <summary>
        /// Gets the aggregated filter expression representing the specification's criteria.
        /// </summary>
        public Expression<Func<T, bool>>? Criteria => _criteria;

        /// <summary>
        /// Gets the list of related entities to include in the query result.
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        /// <summary>
        /// Gets the ordering function to apply to the query.
        /// </summary>
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }

        /// <summary>
        /// Gets the number of records to skip in the result set.
        /// </summary>
        public int? Skip { get; protected set; }

        /// <summary>
        /// Gets the maximum number of records to take from the result set.
        /// </summary>
        public int? Take { get; protected set; }

        /// <summary>
        /// Adds a filter criterion to the specification. If criteria already exist,
        /// the new expression is combined using a logical AND operation.
        /// </summary>
        /// <param name="expression">
        /// The filter expression to apply to the entity type.
        /// </param>
        protected void AddCriteria(Expression<Func<T, bool>> expression)
        {
            _criteria = _criteria == null
                ? expression
                : _criteria.AndAlsoSafe(expression);
        }

        /// <summary>
        /// Adds an include expression to specify a related entity to be eagerly loaded.
        /// </summary>
        /// <param name="includeExpression">
        /// The expression that defines the navigation property to include.
        /// </param>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
            => Includes.Add(includeExpression);

        /// <summary>
        /// Applies pagination parameters to the specification.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        /// <summary>
        /// Applies ordering logic to the specification query.
        /// </summary>
        /// <param name="order">
        /// A function that transforms the queryable into an ordered queryable.
        /// </param>
        protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> order)
        {
            OrderBy = order;
        }
    }
}
