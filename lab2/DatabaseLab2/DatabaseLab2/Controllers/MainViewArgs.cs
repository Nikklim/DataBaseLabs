using DatabaseLab2.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class MainViewArgs
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public int ReaderId { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
        public Reader Reader { get; set; }

        public int RandomCount { get; set; }

        public SearchBookParameters SearchBookParameters { get; set; }
        public SearchAuthorParameters SearchAuthorParameters { get; set; }
        public SearchReaderParameters SearchReaderParameters { get; set; }
    }
}
