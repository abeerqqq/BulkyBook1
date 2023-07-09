using BulkyBook.Models;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        /*
         * This interface include everything in the IRepository Generic class
         * But we have to add update and Svae
         */
        void Update(OrderHeader obj);
       
    }
}
