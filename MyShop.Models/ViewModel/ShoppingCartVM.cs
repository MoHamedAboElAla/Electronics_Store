using MyShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartsList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public decimal TotalCarts { get; set; }
	}
}
