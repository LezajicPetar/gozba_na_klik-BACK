using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class AddressRepository
    {
        private readonly GozbaDbContext _dbContext;

        public AddressRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _dbContext.Addresses
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Address?> GetByIdAndUserIdAsync(int id, int userId)
        {
            return await _dbContext.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<Address> AddAsync(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> UpdateAsync(Address address)
        {
            _dbContext.Addresses.Update(address);
            await _dbContext.SaveChangesAsync();
            return await GetByIdAsync(address.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var address = await GetByIdAsync(id);
            if (address == null)
                return false;

            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByIdAndUserIdAsync(int id, int userId)
        {
            var address = await GetByIdAndUserIdAsync(id, userId);
            if (address == null)
                return false;

            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
