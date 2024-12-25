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
    public class CategoryRepo : GenericRepo<Category>,ICategoryRepo
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context) : base(context)
        {
            _context= context;
        }

        public void Update(Category category)
        {
            var categoryFromDb = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.Name = category.Name;
                categoryFromDb.Description = category.Description;
                categoryFromDb.CreatedTime= DateTime.Now;

            }
            
        }
    }
}
