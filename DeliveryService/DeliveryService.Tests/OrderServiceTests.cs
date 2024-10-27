using DeliveryService.Domain.ViewModels;
using DeliveryService.Service;
using DeliveryService.Service.Implementations;
using DeliveryService.Service.Interfaces;

namespace DeliveryService.Tests
{
    public class OrderServiceTests
    {
        private IOrderService<OrderViewModel> _orderService = new OrderService();

        //Создание заказа
        [Fact]
        public void TryCreateOrder__Test()
        {
            var response = _orderService.Create(
                new OrderViewModel()
                {
                    Weight = 1,
                    DeliveryDateTime = DateTime.Now,
                    DistrictId = 1,
                }).Result;

            Assert.True(response.IsSuccess);
        }

        // Удаление заказа с корректными данными
        [Fact]
        public void TryDeleteOrder_ValidId_Test()
        {
            var response = _orderService.Create(
                new OrderViewModel()
                {
                    Id = -5,
                    Weight = 1,
                    DeliveryDateTime = DateTime.Now,
                    DistrictId = 1,
                }).Result;

            response = _orderService.Delete("-5").Result;

            Assert.True(response.IsSuccess);
        }

        // Удаление заказа с некорректными данными
        [Fact]
        public void TryDeleteOrder_InvalidId_Test()
        {
            var response = _orderService.Delete("as").Result;

            Assert.False(response.IsSuccess);
        }

        // Получение заказа с корректными данными
        [Fact]
        public void TryGetOrder_ValidId_Test()
        {
            var response = _orderService.Create(
                new OrderViewModel()
                {
                    Id = 1,
                    Weight = 1,
                    DeliveryDateTime = DateTime.Now,
                    DistrictId = 1,
                }).Result;

            response = _orderService.Get("1").Result;

            Assert.True(response.IsSuccess);
        }

        // Получение заказа с некорректными данными
        [Fact]
        public void TryGetOrder_InvalidId_Test()
        {
            var response = _orderService.Get("ad").Result;

            Assert.False(response.IsSuccess);
        }

        // Сортировка заказов с корректными данными
        [Fact]
        public void TryToSort_ValidIdAndDate_Test()
        {
            var response = _orderService.Sort("1", "2024-10-27 19:00:00").Result;

            Assert.True(response.IsSuccess);
        }

        // Сортировка заказов с корректным districtId
        [Fact]
        public void TryToSort_InvalidId_Test()
        {
            var response = _orderService.Sort("sda", "2024-10-27 19:00:00").Result;

            Assert.False(response.IsSuccess);
        }

        // Сортировка заказов с корректным Date
        [Fact]
        public void TryToSort_InvalidDate_Test()
        {
            var response = _orderService.Sort("1", "2024:10:27 19:00:00").Result;

            Assert.False(response.IsSuccess);
        }
    }
}