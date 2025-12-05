using gozba_na_klik.Dtos.Queries;
using gozba_na_klik.Model;

namespace gozba_na_klik.Extensions
{
    public static class RestaurantFilteringExtensions
    {
        public static IQueryable<Restaurant> ApplyFiltering(
            this IQueryable<Restaurant> query,
            RestaurantQuery filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var name = filter.Name.Trim().ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(name));
            }

            if (filter.OwnerId is not null)
            {
                query = query.Where(r => r.OwnerId == filter.OwnerId);
            }

            if (filter.MinCapacity is not null)
            {
                query = query.Where(r => r.Capacity >= filter.MinCapacity);
            }

            return query;
        }
    }
}
