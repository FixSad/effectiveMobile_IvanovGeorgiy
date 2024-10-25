using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IOrderService<T>
    {
        Task<IBaseResponse<OrderViewModel>> Create(T viewModel);
        Task<IBaseResponse<OrderViewModel>> Delete(string id);
        Task<IBaseResponse<OrderViewModel>> Get(string id);
        Task<IBaseResponse<List<OrderViewModel>>> Sort(string districtId, string startDateTime);
        Task<IBaseResponse<List<OrderViewModel>>> GetAllOrders();
    }
}
