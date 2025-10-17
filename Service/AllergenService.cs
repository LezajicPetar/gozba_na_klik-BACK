using gozba_na_klik.Data;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Services
{
    public class AllergenService
    {
        private readonly AllergenRepository _allergenRepo;

        public AllergenService(AllergenRepository allergenRepo)
        {
            _allergenRepo = allergenRepo;
        }

        //public async Task<ICollection<Allergen>> GetAllAsync(int userId)
        //{
        //    return await _allergenRepo.GetAllAsync(userId);
        //}
    }
}
