using System.Diagnostics.Metrics;
using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class CourierRepository : ICourierRepository
    {
        public readonly GozbaDbContext _db;

        public CourierRepository(GozbaDbContext db) => _db = db;

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.Role == Enums.Role.Courier)
                .OrderBy(u => u.Id)
                .ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == Enums.Role.Courier);
        }
        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _db.Users
                .AnyAsync(u => u.Id == userId && u.Role == Enums.Role.Courier);
        }
        public async Task<User> EnsureCourierRoleAsync(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId)
                       ?? throw new Exceptions.NotFoundException("User", userId);

            if (user.Role != Enums.Role.Courier)
            {
                user.Role = Enums.Role.Courier;
                await _db.SaveChangesAsync();
            }
            return user;
        }
    }
}
