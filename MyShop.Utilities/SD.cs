using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Utilities
{
    public class SD
    {
        public  const string  AdminRole = "Admin";
        public static string CustomerRole = "Customer";
        public static string EditorRole = "Editor";

        public static string Pending = "Pending";
        public static string Approved = "Approved";
        public static string Rejected = "Rejected";
        public static string Processing = "Processing";
        public static string Shipped = "Shipped";
        public static string Refund = "Refund";
        public static string Cancelled = "Cancelled";

        public const string SessionKey= "ShoppingCartSession";

    }
}
