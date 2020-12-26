using DataBaseLab3.DAL.Model;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3.DAL.Repositories
{
    class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(DbContext context) : base(context)
        {

        }

        public IList<Author> SearchAuthors(string signature, string name, NpgsqlDate minDate)
        {
            return GetAll().Where(x => x.Signature.Contains(signature) && x.Person.Name.Contains(name) && x.Person.BirthdayDate.HasValue && new NpgsqlDate(x.Person.BirthdayDate.Value) > minDate).ToList();
        }

        public bool AddRandomAuthorsToDB(int randomCount)
        {
            context.Database.ExecuteSqlCommandAsync($@"do $$
declare 
   counter integer := 0;
begin
while(counter < {randomCount}) loop
counter:= counter + 1;
    INSERT INTO dbo.""People""
        (""Name"", ""Surname"", ""BirthdayDate"")
    (select(chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)) as name,
             (chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)) as surname,
            ((timestamp '1900-01-10' +
       random() * (timestamp '2020-01-20' -
                   timestamp '1900-01-10'))::DATE)as birthdate);


            INSERT INTO dbo.""Authors""(""Person_PersonId"", ""Signature"")
    SELECT(select MAX(""PersonId"") from dbo.""People""),
	((chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)));
            end loop;

            end$$; ").Wait();
            return true;
        }

        private Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        DateTime RandomDateTime()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
