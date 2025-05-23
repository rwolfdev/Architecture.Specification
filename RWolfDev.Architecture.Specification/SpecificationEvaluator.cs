using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Default implementation of the specification evaluator.
    /// </summary>
    public class SpecificationEvaluator : ISpecificationEvaluator
    {
        public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
        {
            var query = inputQuery;

            // Apply filtering criteria
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply eager loading (includes)
            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }

            // Apply ordering
            if (specification.OrderBy != null)
            {
                query = specification.OrderBy(query);
            }

            // Apply paging
            if (specification.Skip.HasValue)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take.HasValue)
            {
                query = query.Take(specification.Take.Value);
            }

            return query;
        }
    }
}
