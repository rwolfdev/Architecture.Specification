using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Entity Framework implementation of the generic repository using specifications.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class EfRepository<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly ISpecificationEvaluator _specEvaluator;

        public EfRepository(DbContext dbContext, ISpecificationEvaluator specEvaluator)
        {
            _dbContext = dbContext;
            _specEvaluator = specEvaluator;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync() =>
            await _dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var queryable = _specEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
            return await queryable.ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            var queryable = _specEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
