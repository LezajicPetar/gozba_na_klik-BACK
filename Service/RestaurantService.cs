using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using gozba_na_klik.Exceptions;
using AutoMapper;

namespace gozba_na_klik.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository<Restaurant> _repository;
        private readonly IMapper _mapper;

        public RestaurantService(IRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }

        public async Task<RestaurantDto> GetByIdAsync(int id) 
        { 
            var restaurant = await _repository.GetByIdAsync(id);

            if (restaurant == null) throw new NotFoundException("Restaurant", id);

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<RestaurantDto> CreateAsync(RestaurantInputDto inputDto)
        {
            var restaurant = new Restaurant
            {
                Name = inputDto.Name,
                OwnerId = inputDto.OwnerId,
                Photo = inputDto.Photo
            };

            var saved = await _repository.CreateAsync(restaurant);
            var withOwner = await _repository.GetByIdAsync(saved.Id);

            return _mapper.Map<RestaurantDto>(withOwner);
        }

        public async Task<RestaurantDto> UpdateAsync(int id, RestaurantInputDto inputDto)
        {
            var restaurant = await _repository.GetByIdAsync(id);

            if (restaurant == null) throw new NotFoundException("Restaurant", id);

            _mapper.Map(inputDto, restaurant);

            var updated = await _repository.UpdateAsync(restaurant);
            var withOwner = await _repository.GetByIdAsync(updated.Id);

            return _mapper.Map<RestaurantDto>(withOwner);
        }

        public async Task DeleteAsync(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted == null)
                throw new NotFoundException("Restaurant", id);
        }
    }
}
