namespace gozba_na_klik.Model
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T> UpdateAsync(T entity);
        public Task<T> CreateAsync(T entity);
        public Task<T?> DeleteAsync(int id);
    }
}
