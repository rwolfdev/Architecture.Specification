using System.Linq.Expressions;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Defines a specification for querying entities of type <typeparamref name="T"/>.
    /// Supports filtering, eager loading, ordering, and pagination.
    /// </summary>
    /// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the filter expression used to restrict the query results.
        /// Returns <c>null</c> if no filtering is applied.
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Gets the collection of include expressions used for eager loading of related entities.
        /// Each expression defines a navigation property to include.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the ordering function that defines how to sort the query results.
        /// Returns <c>null</c> if no specific order is applied.
        /// </summary>
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }

        /// <summary>
        /// Gets the number of elements to skip in the query result for pagination.
        /// Returns <c>null</c> if pagination is not applied.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets the maximum number of elements to return in the query result for pagination.
        /// Returns <c>null</c> if pagination is not applied.
        /// </summary>
        int? Take { get; }
    }
}
