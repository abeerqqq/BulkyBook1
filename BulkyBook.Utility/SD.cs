using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public static class SD
    {
        // This class hold the constant strings of the roles we have in our project
        public const string Role_Customer = "Customer";
        /*Company user
         * Reistered by Admin 
         * Do not have to makw the payment right away -they can make an order and pay after 30 days max
         */
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
    }
}
