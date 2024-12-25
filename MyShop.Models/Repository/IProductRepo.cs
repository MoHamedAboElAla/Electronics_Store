﻿using MyShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.Repository
{
    public interface IProductRepo : IGenericRepo<Product> 
    {
        void Update(Product product);


    }
}