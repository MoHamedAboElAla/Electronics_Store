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
    public class ProductRepo : GenericRepo<Product>,IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context) : base(context) { 
            _context= context;
        }

        public void Update(Product product)
        {
            var productDb = _context.Products.FirstOrDefault(c => c.Id == product.Id);
            if (productDb != null)
            {
                productDb.Name = product.Name;
                productDb.Description = product.Description;
                productDb.Price = product.Price;
                productDb.ImageUrl = product.ImageUrl;
                productDb.CategoryId = product.CategoryId;



            }
            
        }
    }
}
