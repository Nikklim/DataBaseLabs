using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DbContext context;
        protected IDbSet<T> table;
        public GenericRepository(DbContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }
        public void CreateOrUpdate(T entity)
        {
            table.AddOrUpdate(entity);
            context.SaveChanges();
        }

        public T Get(int id)
        {
            return table.Find(id);
        }

        public IList<T> GetAll()
        {
            return table.ToList();
        }

        public void Remove(T entity)
        {
            table.Remove(entity);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            Remove(Get(id));
        }
    }
}
