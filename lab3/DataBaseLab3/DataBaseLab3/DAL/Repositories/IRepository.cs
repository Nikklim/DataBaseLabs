using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Repositories
{
    interface IRepository<T> where T : class
    {
        T Get(int id);
        IList<T> GetAll();
        void CreateOrUpdate(T entity);
        void Remove(T entity);
        void Remove(int id);
    }
}
