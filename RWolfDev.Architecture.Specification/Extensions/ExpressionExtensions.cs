using System.Linq.Expressions;

namespace RWolfDev.Architecture.Specification.Extensions
{
    /// <summary>
    /// Provides extension methods for safely combining LINQ expression trees using logical operations.
    /// These methods avoid the use of Expression.Invoke to ensure compatibility with Entity Framework Core.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two predicate expressions using a logical AND operation (&&),
        /// using parameter replacement to maintain EF Core compatibility.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
        /// <param name="expr1">The first predicate expression.</param>
        /// <param name="expr2">The second predicate expression.</param>
        /// <returns>
        /// A new expression representing the logical conjunction of <paramref name="expr1"/> and <paramref name="expr2"/>.
        /// </returns>
        public static Expression<Func<T, bool>> AndAlsoSafe<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter),
                ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// Combines two predicate expressions using a logical OR operation (||),
        /// using parameter replacement to maintain EF Core compatibility.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
        /// <param name="expr1">The first predicate expression.</param>
        /// <param name="expr2">The second predicate expression.</param>
        /// <returns>
        /// A new expression representing the logical disjunction of <paramref name="expr1"/> and <paramref name="expr2"/>.
        /// </returns>
        public static Expression<Func<T, bool>> OrElseSafe<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.OrElse(
                ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter),
                ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// Replaces all occurrences of a specific parameter in an expression tree with a new parameter.
        /// </summary>
        /// <param name="body">The expression body to process.</param>
        /// <param name="oldParameter">The parameter to be replaced.</param>
        /// <param name="newParameter">The new parameter to insert.</param>
        /// <returns>
        /// A new expression with the parameter replaced.
        /// </returns>
        private static Expression ReplaceParameter(
            Expression body, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterReplacer(oldParameter, newParameter).Visit(body)!;
        }

        /// <summary>
        /// Helper class to replace parameter expressions within an expression tree.
        /// </summary>
        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterReplacer"/> class.
            /// </summary>
            /// <param name="oldParameter">The parameter to be replaced.</param>
            /// <param name="newParameter">The new parameter to insert.</param>
            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            /// <summary>
            /// Visits the parameter expression and replaces it if it matches the old parameter.
            /// </summary>
            /// <param name="node">The parameter expression node.</param>
            /// <returns>The replaced or original parameter expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}
