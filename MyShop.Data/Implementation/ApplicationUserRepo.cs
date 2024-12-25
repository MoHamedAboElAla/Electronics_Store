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
    public class ApplicationUserRepo : GenericRepo<ApplicationUser>, IApplicationUserRepo
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepo(AppDbContext context) : base(context)
        {
            _context= context;
        }

    
            
        }
    
}
