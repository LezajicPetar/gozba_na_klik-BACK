using gozba_na_klik.Dtos.Queries;
using gozba_na_klik.Model;

namespace gozba_na_klik.Extensions
{
    public static class MenuItemFilteringExtensions
    {
        public static IQueryable<MenuItem> ApplyFiltering(
            this IQueryable<MenuItem> query,
            MenuItemQuery filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var name = filter.Name.Trim().ToLower();
                query = query.Where(i => i.Name.ToLower().Contains(name));
            }

            if (filter.MaxPrice is not null)
            {
                query = query.Where(i => i.Price <= filter.MaxPrice);
            }

            if (filter.MinPrice is not null)
            {
                query = query.Where(i => i.Price >= filter.MinPrice);
            }

            return query;
        }

    }
}
