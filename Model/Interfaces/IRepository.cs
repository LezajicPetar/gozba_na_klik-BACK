namespace gozba_na_klik.Model.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
