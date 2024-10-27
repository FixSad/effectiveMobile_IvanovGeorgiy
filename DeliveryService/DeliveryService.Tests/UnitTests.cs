using DeliveryService.Domain.ViewModels;
using DeliveryService.Service;
using DeliveryService.Service.Implementations;
using DeliveryService.Service.Interfaces;

namespace DeliveryService.Tests
{
    public class UnitTests
    {
        private IOrderService<OrderViewModel> _orderService = new OrderService();
        // Тесты ORDER SERVICE

        [Fact]
        public void TryCreateOrder_PathLogIsGood_Test()
        {
            Properties.Default.DeliveryLog = "\\";
            var response = _orderService.Create(
                new OrderViewModel()
                {
                    Weight = 1,
                    DeliveryDateTime = DateTime.Now,
                    DistrictId = 1,
                }).Result;

            Assert.Equal(true, response.IsSuccess);
        }
    }
}