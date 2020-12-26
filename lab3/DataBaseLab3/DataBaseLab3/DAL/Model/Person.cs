using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Model
{
    class Person
    {
        public int PersonId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime? BirthdayDate { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname} {BirthdayDate}";
        }
    }
}
