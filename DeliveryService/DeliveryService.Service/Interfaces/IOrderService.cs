using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IOrderService<T>
    {
        void Create(T viewModel);
        void Delete(char id);
        IQueryable<T> Get();
    }
}
