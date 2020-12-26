using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class SearchBookParameters
    {
        public string title { get; set; }
        public int minPagesCount { get; set; }
        public int maxPagesCount { get; set; }
    }
}
