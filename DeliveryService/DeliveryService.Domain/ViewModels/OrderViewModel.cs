using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public float Weight { get; set; }
        public int DistrictId { get; set; }
        public DateTime DeliveryDateTime { get; set; }
    }
}
