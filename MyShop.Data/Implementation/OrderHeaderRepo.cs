using MyShop.Models.Models;
using MyShop.Models.Repository;
using MyShop.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.Implementation
{
	public class OrderHeaderRepo : GenericRepo<OrderHeader>, IOrderHeaderRepo
	{
        private readonly AppDbContext _context;
        public OrderHeaderRepo(AppDbContext context) : base(context)
        {
            _context= context;
        }

        public void Update(OrderHeader orderHeader)
        {
           _context.OrderHeaders.Update(orderHeader);

		}

		public void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus)
		{
			var orderFromDb = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
			if (orderFromDb != null)
			{
				orderFromDb.OrderStatus = OrderStatus;
				orderFromDb.PaymentDate = DateTime.Now;
				if (PaymentStatus != null)
				{
					orderFromDb.PaymentStatus = PaymentStatus;
				}
			}
		}
	}
}
