using MyShop.Models.Repository;
using MyShop.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Data.Implementation
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _context;
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IShoppingCartRepo ShoppingCart { get; private set; }
        public IOrderDetailsRepo OrderDetails { get; private set; }
		public IOrderHeaderRepo OrderHeader { get; private set; }
        public IApplicationUserRepo ApplicationUser { get; private set; }

		public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Category = new CategoryRepo(_context);
            Product = new ProductRepo(_context);
            ShoppingCart = new ShoppingCartRepo(_context);
			OrderDetails = new OrderDetailsRepo(_context);
			OrderHeader = new OrderHeaderRepo(_context);
            ApplicationUser = new ApplicationUserRepo(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
