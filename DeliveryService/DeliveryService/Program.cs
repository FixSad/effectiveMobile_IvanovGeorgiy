using DeliveryService.Domain.ViewModels;
using DeliveryService.Service.Interfaces;
using DeliveryService.Service.Implementations;

IOrderService<OrderViewModel> _orderRepository = new OrderService();

_orderRepository.Create(new OrderViewModel()
{
    Id = 2,
    Weight = 1,
    DistrictId = 1,
    DeliveryDateTime = DateTime.Now
});

//_orderRepository.Delete('2');