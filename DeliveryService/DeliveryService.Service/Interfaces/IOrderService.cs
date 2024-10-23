using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IOrderService<T>
    {
        Task<IBaseResponse<OrderViewModel>> Create(T viewModel);
        Task<IBaseResponse<OrderViewModel>> Delete(char id);
        Task<IBaseResponse<OrderViewModel>> Get(char id);
    }
}
