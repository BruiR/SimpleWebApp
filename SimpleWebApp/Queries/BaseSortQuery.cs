namespace SimpleWebApp.Queries
{
    public class BaseSortQuery<TEntity>
    {
        public IQueryable<TEntity> ApplySort(IQueryable<TEntity> source, IEnumerable<SortExpression<TEntity>> sortRules)
        {
            var firstRule = sortRules.First();
            var orderSource = firstRule.IsAscending
                ? source.OrderBy(firstRule.Selector) 
                : source.OrderByDescending(firstRule.Selector);

            return sortRules.Skip(1)
                .Aggregate(orderSource, (current, sortRule) => sortRule.IsAscending
                ? current.ThenBy(sortRule.Selector)
                : current.ThenByDescending(sortRule.Selector));
        }
    }
}
