using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class SearchAuthorParameters
    {
        public string Signature { get; set; }
        public string Name { get; set; }
        public NpgsqlTypes.NpgsqlDate MinDate { get; set; }
    }
}
