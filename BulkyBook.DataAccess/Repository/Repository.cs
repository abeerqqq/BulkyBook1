using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    /* When Making a modification here -> all the classes implement this will be updated as well 
     * RP pros
     
     */
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        // Internal members are accessible only within files in the same assembly (.dll).
        // access is limited exclusively to classes defined within the current project assembly
        /*DbSet 
         * it is a class that has all the CRUD methods in EF
         * it represenet an entity 
         * categories is created by using this class -> DbSet<category> categories;
         * Whenerver we wnat to apply CRUD we do this
         *  _db.categories.Add(obj);
         *  By default the CRUD added to the categories intatnce 
         */
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = db.Set<T>(); // we define this becasue it is eaiser than typing -> -db.DbSet<t>().Add(T entity)
            // We can just -> dbSet.Add(T enitiy)
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            /**/
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            /**/
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
