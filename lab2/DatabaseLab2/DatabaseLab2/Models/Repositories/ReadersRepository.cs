using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Models.Repositories
{
    class ReadersRepository : BaseRepository
    {
        public ReadersRepository(string connectionString) : base(connectionString)
        {
        }

        #region BaseCRUD
        public IEnumerable<Reader> GetAllReaders()
        {
            List<Reader> readers = new List<Reader>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("select reader_id, favouritegenre, name, surname, birthdate from readers join persons on persons.person_id = readers.reader_id", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                readers.Add(ReadReader(reader));
            }

            con.Close();
            return readers;
        }

        public Reader GetReader(int id)
        {
            Reader reader1 = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"select reader_id, favouritegenre, name, surname, birthdate from readers join persons on persons.person_id = readers.reader_id where readers.reader_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                reader1 = ReadReader(reader);
            }

            con.Close();
            return reader1;
        }
        public bool UpdateReader(Reader reader1)
        {
            try
            {
                bool res = false;
                con.Open();
                var transaction = con.BeginTransaction();

                NpgsqlCommand commandUpdatePerson = new NpgsqlCommand($"update persons set name = @name, surname = @surname, birthdate = @birthdate where person_id = {reader1.Id}", con);

                commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@name", reader1.Person.Name));
                commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@surname", reader1.Person.Surname));
                if (reader1.Person.BirthDate != null)
                {
                    commandUpdatePerson.Parameters.Add(new NpgsqlParameter("@birthdate", reader1.Person.BirthDate.Value));
                }
                else
                {
                    commandUpdatePerson.CommandText = $"update persons set name = @name, surname = @surname, birthdate = null where person_id = {reader1.Id}";
                }

                if (commandUpdatePerson.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                NpgsqlCommand commandUpdateReader = new NpgsqlCommand($"update readers set favouritegenre = @favouritegenre where reader_id = {reader1.Id}", con);

                commandUpdateReader.Parameters.Add(new NpgsqlParameter("@favouritegenre", reader1.FavouriteGenre));

                if (commandUpdateReader.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                transaction.Commit();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }

        }
        public bool InsertReader(Reader reader1)
        {
            try
            {
                bool res = false;
                con.Open();


                try
                {
                    NpgsqlCommand commandInsertPerson = new NpgsqlCommand($"insert into persons (name, surname, birthdate) values(@name, @surname, @birthdate) returning person_id", con);

                    commandInsertPerson.Parameters.Add(new NpgsqlParameter("@name", reader1.Person.Name));
                    commandInsertPerson.Parameters.Add(new NpgsqlParameter("@surname", reader1.Person.Surname));
                    if (reader1.Person.BirthDate != null)
                    {
                        commandInsertPerson.Parameters.Add(new NpgsqlParameter("@birthdate", reader1.Person.BirthDate));
                    }
                    else
                    {
                        commandInsertPerson.CommandText = $"insert into persons (name, surname) values(@name, @surname) returning person_id";
                    }

                    reader1.Id = Convert.ToInt32(commandInsertPerson.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    try
                    {

                        NpgsqlCommand checkIsPersonReadyCommand = new NpgsqlCommand($"select person_id from persons where name = @name and surname = @surname and birthdate = @birthdate", con);

                        checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@name", reader1.Person.Name));
                        checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@surname", reader1.Person.Surname));
                        if (reader1.Person.BirthDate == null)
                        {
                            checkIsPersonReadyCommand.CommandText = $"select person_id from persons where name = @name and surname = @surname and birthdate is null";
                        }
                        else
                        {
                            checkIsPersonReadyCommand.Parameters.Add(new NpgsqlParameter("@birthdate", reader1.Person.BirthDate));
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
                                reader1.Id = id;
                            }
                        }
                    }
                    catch { }
                }

                NpgsqlCommand command = new NpgsqlCommand($"insert into readers (reader_id, favouritegenre) values({reader1.Id}, @favouritegenre)", con);
                command.Parameters.Add(new NpgsqlParameter("@favouritegenre", reader1.FavouriteGenre));

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                con.Close();
                return false;
            }


        }
        public bool DeleteReader(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from readers where reader_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }
        #endregion

        public IEnumerable<Book> GetReadersBooks(int readerId)
        {
            con.Open();
            List<Book> books = new List<Book>();

            NpgsqlCommand command = new NpgsqlCommand($"select * from books where book_id in (select book_id from linksreadertobook where reader_id = {readerId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                books.Add(ReadBook(reader));
            }
            con.Close();
            return books;
        }
        public IEnumerable<Reader> SearchReaders(string favouritegenre, string name, string surname)
        {
            List<Reader> readers = new List<Reader>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($@"select reader_id, favouritegenre, name, surname, birthdate from readers join persons on persons.person_id = readers.reader_id
            where
			favouritegenre like '%{favouritegenre}%' and
			name like '%{name}%' and
			surname like '%{surname}%'", con);
            


            var reader = command.ExecuteReader();

            while (reader.Read())
            {

                readers.Add(ReadReader(reader));
            }

            con.Close();
            return readers;
        }
    }
}
