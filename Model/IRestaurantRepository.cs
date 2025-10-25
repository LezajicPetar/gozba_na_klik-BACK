namespace gozba_na_klik.Model
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<Restaurant?> GetByIdWithOwnerAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllWithOwnersAsync();
    }
}
