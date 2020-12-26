using DataBaseLab3.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Repositories
{
    class BookRepository : GenericRepository<Book>
    {
        public BookRepository(DbContext context) : base(context)
        {

        }

        public IList<Book> SearchBooks(string title, int minPagesCount, int maxPagesCount)
        {
            return GetAll().Where(x => x.Title.Contains(title) && x.PagesCount >= minPagesCount && x.PagesCount <= maxPagesCount).ToList();
        }
    }
}
