using System.Linq.Expressions;

namespace SimpleWebApp.Domain.Queries
{
    public class SortExpression<TEntity>
    {
        public bool IsAscending { get; set; }

        public Expression<Func<TEntity, object>> Selector { get; set; }
    }
}
