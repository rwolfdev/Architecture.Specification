using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Generic repository abstraction for specification-based querying.
    /// </summary>
    /// <typeparam name="T">The aggregate root entity type.</typeparam>
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Lists all entities of the specified type.
        /// </summary>
        Task<IReadOnlyList<T>> ListAllAsync();

        /// <summary>
        /// Lists all entities matching the given specification.
        /// </summary>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        /// <summary>
        /// Gets a single entity matching the given specification or null.
        /// </summary>
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates the given entity in the repository.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes the given entity from the repository.
        /// </summary>
        Task DeleteAsync(T entity);
    }
}
