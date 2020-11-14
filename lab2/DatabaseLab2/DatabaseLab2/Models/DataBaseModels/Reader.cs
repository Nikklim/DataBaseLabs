using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Reader
    {
        public int Id { get; set; }

        public string FavouriteGenre { get; set; } = "";

        public virtual Person Person { get; set; }

        public override string ToString()
        {
            return $"{Id} {FavouriteGenre} {Person}";
        }

    }
}
