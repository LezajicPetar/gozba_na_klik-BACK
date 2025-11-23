using AutoMapper;
using gozba_na_klik.Enums;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Service.Interfaces;

namespace gozba_na_klik.Service.Implementations
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
                Status = OrderStatus.NA_CEKANJU,
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
        public async Task AcceptAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order", orderId);

            if (order.Status != OrderStatus.NA_CEKANJU)
                throw new BadRequestException("Order is not pending.");

            order.Status = OrderStatus.PRIHVACENA;

            await _orderRepo.UpdateAsync(order);
        }
        public async Task RejectAsync(int orderId, RejectOrderDto? dto = null)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                ?? throw new NotFoundException("Order", orderId);

            // Dozvoljava odbijanje samo ako nije krenula isporuka AZ
            if (order.Status != OrderStatus.NA_CEKANJU &&
                order.Status != OrderStatus.PRIHVACENA)
            {
                throw new BadRequestException("Porudzbinu nije moguce odbiti u trenutnom statusu");
            }

            order.Status = OrderStatus.OTKAZANA;

            await _orderRepo.UpdateAsync(order);
        }
        public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<List<OrderDto>> GetPendingAsync(CancellationToken ct = default)
        {
            var orders = await _orderRepo.GetPendingAsync(ct);
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
