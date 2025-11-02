using AutoMapper;
using gozba_na_klik.Enums;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model;
using gozba_na_klik.Dtos.Order;

namespace gozba_na_klik.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepo,
            ILogger<OrderService> logger,
            IMapper mapper,
            IRestaurantRepository restaurantRepo)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _mapper = mapper;
            _restaurantRepo = restaurantRepo;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var restaurant = await EnsureRestaurantExists(dto.RestaurantId);

            var subtotal = dto.Items.Sum(i => i.Price);
            var deliveryFee = 200;
            var total = subtotal + deliveryFee;

            var order = new Order
            {
                RestaurantId = dto.RestaurantId,
                CustomerId = dto.CustomerId,
                AddressId = dto.AddressId,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                Total = total,
                Status = OrderStatus.NaCekanju,
                Items = dto.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Price = i.Price
                }).ToList()
            };

            var created = await _orderRepo.CreateAsync(order);

            return _mapper.Map<OrderDto>(created);
        }

        private async Task<Restaurant> EnsureRestaurantExists(int id)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(id);

            if (restaurant == null) { throw new NotFoundException("Restaurant", id); }

            return restaurant;
        }
    }
}
