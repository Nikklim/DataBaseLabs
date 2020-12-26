using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Model
{
    class Book
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        public int PagesCount { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Reader> Readers { get; set; }

        public override string ToString()
        {
            return $"{BookId}  {Title}  {PagesCount}";
        }
    }
}
