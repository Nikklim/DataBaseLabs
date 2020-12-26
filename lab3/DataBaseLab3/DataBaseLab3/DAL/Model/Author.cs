using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Model
{
    class Author
    {
        public int AuthorId { get; set; }
        [Required]
        public string Signature { get; set; }
        [Required]
        public Person Person { get; set; }

        public ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return $"{AuthorId} {Signature} {Person}";
        }
    }
}
