using gozba_na_klik.Model;

namespace gozba_na_klik.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }

        public static AddressDto CreateDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                Street = address.Street,
                HouseNumber = address.HouseNumber,
                City = address.City,
                UserId = address.UserId
            };
        }
    }

    public class AddressInputDto
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
    }

    public class AddressUpdateDto
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }
    }
}
