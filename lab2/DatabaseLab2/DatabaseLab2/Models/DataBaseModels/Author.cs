﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.DataBaseModels
{
    class Author
    {
        public int Id { get; set; }

        public string Signature { get; set; }

        public virtual Person Person { get; set; }

        public override string ToString()
        {
            return $"{Id} {Signature} {Person}";
        }
    }
}
