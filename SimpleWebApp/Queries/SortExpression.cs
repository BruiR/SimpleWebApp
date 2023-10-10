using System.Linq.Expressions;

namespace SimpleWebApp.Queries
{
    public class SortExpression<TEntity>
    {
        public bool IsAscending { get; set; }

        public Expression<Func<TEntity, object>> Selector { get; set; }
    }
}
