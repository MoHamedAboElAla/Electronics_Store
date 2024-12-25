using MyShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.Repository
{
    public interface IOrderHeaderRepo: IGenericRepo<OrderHeader> 
    {
        void Update(OrderHeader orderHeader);
        void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus);


    }
}
