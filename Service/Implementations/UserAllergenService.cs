using gozba_na_klik.Model.Entities;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Service.Implementations
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
