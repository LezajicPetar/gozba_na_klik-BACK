using System.Linq.Expressions;

namespace gozba_na_klik.Extensions
{
    public static class QueryableSortingExtensions
    {
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string? sortBy,
            string? sortDir,
            Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (string.IsNullOrWhiteSpace(sortBy) ||
                !columnsMap.TryGetValue(sortBy.ToLower(), out var keySelector))
            {
                return query;
            }

            var descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

            return descending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
    }
}
