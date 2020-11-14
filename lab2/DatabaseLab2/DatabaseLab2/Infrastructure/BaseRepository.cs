using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Infrastructure
{
    abstract class BaseRepository
    {
        protected readonly NpgsqlConnection con;

        protected Person ReadPerson(NpgsqlDataReader reader)
        {
            NpgsqlDate? date = null;
            string birthdate = reader["birthdate"].ToString();
            if (birthdate.Length != 0)
            {
                var dateTime = DateTime.Parse(birthdate);
                date = new NpgsqlDate(dateTime.Date);
            }

            return new Person
            {
                Name = reader["name"].ToString(),
                Surname = reader["surname"].ToString(),
                BirthDate = date
            };
        }


        protected Book ReadBook(NpgsqlDataReader reader)
        {
            return new Book
            {
                Id = Convert.ToInt32(reader["book_id"]),
                PagesCount = Convert.ToInt32(reader["pagescount"]),
                Title = reader["title"].ToString()
            };
        }

        protected Author ReadAuthor(NpgsqlDataReader reader)
        {
            return new Author
            {
                Id = Convert.ToInt32(reader["author_id"]),
                Signature = reader["signature"].ToString(),
                Person = ReadPerson(reader)
            };
        }

        protected Reader ReadReader(NpgsqlDataReader reader)
        {
            return new Reader
            {
                Id = Convert.ToInt32(reader["reader_id"]),
                FavouriteGenre = reader["favouritegenre"].ToString(),
                Person = ReadPerson(reader)
            };
        }

        public BaseRepository(string connectionString)
        {
            con = new NpgsqlConnection(connectionString);
        }
    }
}
