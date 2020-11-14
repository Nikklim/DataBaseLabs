using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int PagesCount { get; set; }

        public override string ToString()
        {
            return $"{Id}  {Title}  {PagesCount}";
        }
    }
}
