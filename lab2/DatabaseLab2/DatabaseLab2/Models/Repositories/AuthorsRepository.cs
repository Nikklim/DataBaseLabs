using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.Repositories
{
    class AuthorsRepository : BaseRepository
    {

        public AuthorsRepository(string connectionString) : base(connectionString)
        {
        }

        #region BaseCRUD
        public IEnumerable<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("select author_id, name, surname, birthdate, signature from authors join persons on persons.person_id = authors.author_id", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                
                authors.Add(ReadAuthor(reader));
            }

            con.Close();
            return authors;
        }

        public Author GetAuthor(int id)
        {
            Author author = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"select author_id, name, surname, birthdate, signature from authors join persons on persons.person_id = authors.author_id where authors.author_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                author = ReadAuthor(reader);
            }

            con.Close();
            return author;
        }
        public bool UpdateAuthor(Author author) 
        {
            try
            {
                bool res = false;
                con.Open();
                var transaction = con.BeginTransaction();
                
                NpgsqlCommand commandUpdatePerson = new NpgsqlCommand($"update persons set name = @name, surname = @surname, birthdate = @birthdate where person_id = {author.Id}", con);

                commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@name", author.Person.Name));
                commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@surname", author.Person.Surname));
                if (author.Person.BirthDate != null)
                {
                    commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@birthdate", author.Person.BirthDate.Value));
                }
                else
                {
                    commandUpdatePerson.CommandText = $"update persons set name = @name, surname = @surname, birthdate = null where person_id = {author.Id}";
                }

                if (commandUpdatePerson.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                NpgsqlCommand commandUpdateAuthor = new NpgsqlCommand($"update authors set signature = @signature where author_id = {author.Id}", con);

                commandUpdateAuthor.Parameters.Add(new NpgsqlParameter("@signature", author.Signature));

                if (commandUpdateAuthor.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                transaction.Commit();
                con.Close();
                return res;
            }
            catch(Exception ex)
            {
                con.Close();
                return false;
            }
        }
        public bool InsertAuthor(Author author)
        {
            try
            {
                bool res = false;
                con.Open();


                try
                {
                    NpgsqlCommand commandInsertPerson = new NpgsqlCommand($"insert into persons (name, surname, birthdate) values(@name, @surname, @birthdate) returning person_id", con);

                    commandInsertPerson.Parameters.Add(new NpgsqlParameter("@name", author.Person.Name));
                    commandInsertPerson.Parameters.Add(new NpgsqlParameter("@surname", author.Person.Surname));
                    if (author.Person.BirthDate != null)
                    {
                        commandInsertPerson.Parameters.Add(new NpgsqlParameter("@birthdate", author.Person.BirthDate));
                    }
                    else
                    {
                        commandInsertPerson.CommandText = $"insert into persons (name, surname) values(@name, @surname) returning person_id";
                    }

                    author.Id = Convert.ToInt32(commandInsertPerson.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    try
                    {
                        
                        NpgsqlCommand checkIsPersonReadyCommand = new NpgsqlCommand($"select person_id from persons where name = @name and surname = @surname and birthdate = @birthdate", con);

                        checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@name", author.Person.Name));
                        checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@surname", author.Person.Surname));
                        if (author.Person.BirthDate == null)
                        {
                            checkIsPersonReadyCommand.CommandText = $"select person_id from persons where name = @name and surname = @surname and birthdate is null";
                        }
                        else
                        {
                            checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@birthdate", author.Person.BirthDate));
                        }


                        using (var reader = checkIsPersonReadyCommand.ExecuteReader())
                        {
                            int id = -1;
                            if (reader.Read())
                            {
                                id = Convert.ToInt32(reader["person_id"]);
                            }
                            if (id != -1)
                            {
                                author.Id = id;
                            }
                        }
                    }
                    catch { }
                }

                NpgsqlCommand command = new NpgsqlCommand($"insert into authors (author_id, signature) values({author.Id}, @signature)", con);
                command.Parameters.Add(new NpgsqlParameter("@signature", author.Signature));

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                con.Close();
                return false;
            }
            
        }
        public bool DeleteAuthor(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from authors where author_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }
        #endregion

        public IEnumerable<Book> GetAuthorsBooks(int authorId)
        {
            con.Open();
            List<Book> books = new List<Book>();

            NpgsqlCommand command = new NpgsqlCommand($"select * from books where book_id in (select book_id from linksbooktoauthor where author_id = {authorId})", con);

            var reader = command.ExecuteReader();

            while(reader.Read())
            {
                books.Add(ReadBook(reader));
            }
            con.Close();
            return books;
        }
        public bool AddRandomAuthorsToDB(int count)
        {
            if (count <= 0)
                return false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"do $$
declare 
   counter integer := 0;
begin
while(counter < {count}) loop
counter:= counter + 1;
    INSERT INTO persons
        (name, surname, birthdate)
    (select(chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)) as name,
             (chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)) as surname,
            ((timestamp '1900-01-10' +
       random() * (timestamp '2020-01-20' -
                   timestamp '1900-01-10'))::DATE)as birthdate);


            INSERT INTO authors(author_id, signature)
    SELECT(select MAX(person_id) from persons),
	((chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)|| chr(trunc(65 + random() * 25)::int)|| chr(trunc(65 + random() * 25)::int)|| chr(trunc(65 + random() * 25)::int)|| chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int)));
            end loop;

            end$$; ", con);

            command.ExecuteScalar();
            //int c = command.ExecuteNonQuery();

            con.Close();
            return true;
        }
        public IEnumerable<Author> SearchAuthors(string signature, string name, NpgsqlDate minDate)
        {
            List<Author> authors = new List<Author>();
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"select author_id, name, surname, birthdate, signature from authors join persons on persons.person_id = authors.author_id
            where
			signature like '%{signature}%' and
			name like '%{name}%' and
			birthdate > @date", con);

            command.Parameters.Add(new NpgsqlParameter("@date", minDate));
            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                authors.Add(ReadAuthor(reader));
            }

            con.Close();
            return authors;
        }
    }
}
