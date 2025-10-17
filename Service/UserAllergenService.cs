using gozba_na_klik.Model;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Services
{
    public class UserAllergenService
    {
        private readonly UserAllergenRepository _userAllergenRepo;

        public UserAllergenService(UserAllergenRepository userAllergenRepo)
        {
            _userAllergenRepo = userAllergenRepo;
        }

        public async Task<ICollection<Allergen>?> GetAllByUserAsync(int userId)
        {
            return await _userAllergenRepo.GetAllByUserAsync(userId);
        }
    }
}
