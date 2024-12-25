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
    public class ShoppingCartRepo : GenericRepo<ShoppingCart>, IShoppingCartRepo
    {
        private readonly AppDbContext _context;
        public ShoppingCartRepo(AppDbContext context) : base(context)
        {
            _context= context;
        }

        public void Update(ShoppingCart shoppingCart)
        {
           _context.Update(shoppingCart);
        }
    }
}
