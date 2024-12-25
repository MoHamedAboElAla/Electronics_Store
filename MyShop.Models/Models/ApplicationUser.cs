using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Models.Models
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

    }
}
