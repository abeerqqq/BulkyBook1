using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        /*
         * This is our generic Reposiroty
         * WE got insight of which functions to inculde when viewing CategroyController
         */

        /*
         * GetFirstOrDefault return an Object of type T 
         */
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        /*
         * GetAll return a list of type T
         */
        IEnumerable<T> GetAll();
        /*
        * Add return Nothing but add an object of type T
        */
        void Add(T entity);
        /*
         * Remove return Nothing but remove an object of type T
         */
        void Remove(T entity);
        /*
        * RemoveRange return Nothing but remove a list of objects
        */
        void RemoveRange(IEnumerable<T> entities);

        /* Update ?
         * Ipdate is not smth common for all the repositories 
         * Example: Uodating category may have diffrent logic from updating Product
         * Product include image and Catgrory Dosent
         * Make Sense NOT to include update in generic repo .. it has a specefic logic for each
         */
    }
}
