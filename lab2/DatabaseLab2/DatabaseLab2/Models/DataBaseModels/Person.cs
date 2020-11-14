using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{

    class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public NpgsqlDate? BirthDate { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} {BirthDate}";
        }
    }
}
