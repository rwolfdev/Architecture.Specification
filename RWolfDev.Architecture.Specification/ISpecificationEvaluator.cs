using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWolfDev.Architecture.Specification
{
    /// <summary>
    /// Evaluates specifications and applies them to IQueryable collections.
    /// </summary>
    public interface ISpecificationEvaluator
    {
        /// <summary>
        /// Applies a specification to a queryable source.
        /// </summary>
        IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class;
    }
}
