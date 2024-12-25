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
	public class OrderDetailsRepo : GenericRepo<OrderDetails>, IOrderDetailsRepo
	{
        private readonly AppDbContext _context;
        public OrderDetailsRepo(AppDbContext context) : base(context)
        {
            _context= context;
        }

        public void Update(OrderDetails orderDetails)
        {
           _context.OrderDetails.Update(orderDetails);

		}

	
	}
}
