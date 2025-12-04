using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly GozbaDbContext _db;

        public EmployeeRepository(GozbaDbContext db)
        {
            _db = db;
        }

        public Task<List<Employee>> GetByRestaurantIdAsync(int restaurantId)
        {
            return _db.Employees
                .Where(e => e.RestaurantId == restaurantId)
                .OrderBy(e => e.LastName)
                .ToListAsync();
        }

        public Task<Employee?> GetAsync(int id)
        {
            return _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<Employee?> GetByEmailAsync(string email)
        {
            return _db.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }


        public async Task<Employee> CreateAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var e = await GetAsync(id);
            if (e == null) return;

            e.Status = (e.Status == EmployeeStatus.Active)
                ? EmployeeStatus.Suspended
                : EmployeeStatus.Active;

            await _db.SaveChangesAsync();
        }
    }
}
