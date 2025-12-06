using gozba_na_klik.Dtos;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Service
{
    public class AddressService
    {
        private readonly AddressRepository _addressRepository;

        public AddressService(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<List<AddressDto>> GetByUserIdAsync(int userId)
        {
            var addresses = await _addressRepository.GetByUserIdAsync(userId);
            return addresses.Select(AddressDto.CreateDto).ToList();
        }

        public async Task<AddressDto?> GetByIdAsync(int id, int userId)
        {
            var address = await _addressRepository.GetByIdAndUserIdAsync(id, userId);
            return address != null ? AddressDto.CreateDto(address) : null;
        }

        public async Task<AddressDto?> CreateAsync(AddressInputDto dto, int userId)
        {
            var address = new Address
            {
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                City = dto.City,
                UserId = userId
            };

            var createdAddress = await _addressRepository.AddAsync(address);
            return AddressDto.CreateDto(createdAddress);
        }

        public async Task<AddressDto?> UpdateAsync(int id, AddressUpdateDto dto, int userId)
        {
            var address = await _addressRepository.GetByIdAndUserIdAsync(id, userId);
            if (address == null)
                return null;

            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            address.City = dto.City;

            var updatedAddress = await _addressRepository.UpdateAsync(address);
            return updatedAddress != null ? AddressDto.CreateDto(updatedAddress) : null;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            return await _addressRepository.DeleteByIdAndUserIdAsync(id, userId);
        }
    }
}
