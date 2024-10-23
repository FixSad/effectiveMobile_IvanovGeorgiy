using DeliveryService.Domain.ViewModels;
using DeliveryService.Service.Interfaces;
using DeliveryService.Service.Implementations;

IOrderService<OrderViewModel> _orderRepository = new OrderService();

/*var response = _orderRepository.Create(new OrderViewModel()
{
    Weight = 1,
    DistrictId = 1,
    DeliveryDateTime = DateTime.Now
}).Result;*/

var response = _orderRepository.Delete('2').Result;

Console.WriteLine(response.Description);
