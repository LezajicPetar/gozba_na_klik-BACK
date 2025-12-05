using gozba_na_klik.Dtos.Queries;
using gozba_na_klik.Model;

namespace gozba_na_klik.Extensions
{
    public static class UserFilteringExtensions
    {
        public static IQueryable<User> ApplyFiltering(
            this IQueryable<User> query,
            UserQuery filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                var firstName = filter.FirstName.Trim().ToLower();
                query = query.Where(u => u.FirstName == firstName);
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                var lastName = filter.LastName.Trim().ToLower();
                query = query.Where(u => u.LastName == lastName);
            }

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                var username = filter.Username.Trim().ToLower();
                query = query.Where(u => u.Username == username);
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                var email = filter.Email.Trim().ToLower();
                query = query.Where(u => u.Email == email);
            }

            if (filter.Role is not null)
            {
                query = query.Where(u => u.Role == filter.Role);
            }

            return query;
        }

    }
}
