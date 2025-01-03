﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.Repository
{
    public interface IUnitOfWork :IDisposable
    {
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        IShoppingCartRepo ShoppingCart { get; }
		IOrderDetailsRepo OrderDetails { get; }
		IOrderHeaderRepo OrderHeader { get; }
        IApplicationUserRepo ApplicationUser { get; }

        int Complete();
    }
}
