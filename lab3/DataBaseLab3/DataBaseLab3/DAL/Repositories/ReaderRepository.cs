using DataBaseLab3.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Repositories
{
    class ReaderRepository : GenericRepository<Reader>
    {
        public ReaderRepository(DbContext context) : base(context)
        {

        }
        public IList<Reader> SearchReaders(string favouritegenre, string name, string surname)
        {
            var items = GetAll().Where(x => x.FavouriteGenre.Contains(favouritegenre) && x.Person.Name.Contains(name) && x.Person.Surname.Contains(surname)).ToList();
            return items;
        }
    }
}
