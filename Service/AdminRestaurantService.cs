using System.Threading;
using AutoMapper;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;

namespace gozba_na_klik.Service
{
    public class AdminRestaurantService : IAdminRestaurantService
    {
        private readonly IRestaurantRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminRestaurantService> _logger;

        public AdminRestaurantService(IRestaurantRepository repo, IMapper mapper, ILogger<AdminRestaurantService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all restaurants");
            var entities = await _repo.GetAllWithOwnersAsync();
            return _mapper.Map<IEnumerable<RestaurantDto>>(entities);
        }

        public async Task<RestaurantDto> GetByIdAsync(int id)
        {
            _logger.LogDebug("Fetching restaurant {Id}", id);
            var entity = await _repo.GetByIdWithOwnerAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Restaurant {Id} not found", id);
                throw new NotFoundException("Restaurant", id);
            }
            return _mapper.Map<RestaurantDto>(entity);
        }

        public async Task<RestaurantDto> CreateAsync(RestaurantInputDto input)
        {
            _logger.LogInformation("Creating restaurant for OwnerId={OwnerId}", input.OwnerId);
           
             var owner = await _repo.GetByIdAsync(input.OwnerId);
             if (owner == null)
                throw new BadRequestException($"Owner with Id {input.OwnerId} does not exist.");

            var entity = _mapper.Map<Restaurant>(input);
            var saved = await _repo.CreateAsync(entity);
            var result = await _repo.GetByIdWithOwnerAsync(saved.Id);

            _logger.LogInformation("Restaurant {Id} created", saved.Id);
            return _mapper.Map<RestaurantDto>(result);
        }

        public async Task<RestaurantDto> UpdateAsync(int id, RestaurantInputDto input)
        {
            _logger.LogInformation("Updating restaurant {Id}", id);
            var entity = await _repo.GetByIdWithOwnerAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Restaurant {Id} not found", id);
                throw new NotFoundException("Restaurant", id);
            }
            _mapper.Map(input, entity);
            var updated = await _repo.UpdateAsync(entity);
            var withOwner = await _repo.GetByIdWithOwnerAsync(updated.Id);
            _logger.LogInformation("Restaurant {Id} updated", id);
            return _mapper.Map<RestaurantDto>(withOwner);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting restaurant {Id}", id);
            var deleted = await _repo.DeleteAsync(id);
            if (deleted == null)
            {
                _logger.LogWarning("Restaurant {Id} not found for delete", id);
                throw new NotFoundException("Restaurant", id);
            }
            _logger.LogInformation("Restaurant {Id} deleted", id);
        }
    }
}