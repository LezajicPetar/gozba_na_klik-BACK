using gozba_na_klik.Dtos;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly AllergenRepository _allergenRepository;
        private readonly UserAllergenRepository _userAllergenRepository;

        public UserService(UserRepository userRepository, AllergenRepository allergenRepository, UserAllergenRepository userAllergenRepository)
        {
            _userRepository = userRepository;
            _allergenRepository = allergenRepository;
            _userAllergenRepository = userAllergenRepository;
        }

        public async Task<User?> UpdateUserAsync(int userId, UpdateCustomerDto dto)
        {
            var allAllergens = await _allergenRepository.GetAllAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;


            var newUserAllergens = allAllergens
                    .Where(a => dto.Allergens.Contains(a.Name))
                    .Select(a => new UserAllergen
                    {
                        UserId = user.Id,
                        AllergenId = a.Id
                    })
                    .ToList();

                await _userAllergenRepository.ReplaceUserAllergens(user, newUserAllergens);

            return await _userRepository.UpdateAsync(user);
        }
    }
}
