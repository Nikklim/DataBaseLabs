using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using DatabaseLab2.Infrastructure;
using DatabaseLab2.Models.DataBaseModels;
using Npgsql;
using NpgsqlTypes;

namespace DatabaseLab2.Models.Repositories
{
    class BooksRepository : BaseRepository
    {
        public BooksRepository(string connectionString) : base(connectionString)
        {
        }

        #region BaseCRUD
        public IEnumerable<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("Select * from books", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                books.Add(ReadBook(reader));
            }

            con.Close();
            return books;
        }

        public Book GetBook(int id)
        {
            Book book = null;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"Select * from books where book_id = {id}", con);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                book = ReadBook(reader);
            }

            con.Close();
            return book;
        }
        public bool UpdateBook(Book book)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"update books set pagescount = {book.PagesCount}, title = @title where book_id = {book.Id}", con);

                command.Parameters.Add(new NpgsqlParameter("@title", book.Title));

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertBook(Book book)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"insert into books (pagescount, title) values({book.PagesCount}, @title)", con);

                command.Parameters.Add(new NpgsqlParameter("@title", book.Title));

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                con.Close();
                return false;
            }
        }
        public bool DeleteBook(int id)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from books where book_id = {id}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }
            con.Close();
            return res;
        }
        #endregion

        #region booksWithAuthors
        public bool AddAuthorToBook(int authorId, int bookId)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"insert into linksbooktoauthor (book_id, author_id) values({bookId}, {authorId})", con);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                con.Close();
                return false;
            }


        }
        public bool DeleteAuthorFromBook(int authorId, int bookId)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from linksbooktoauthor where book_id = {bookId} and author_id = {authorId}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }
        public IEnumerable<Author> GetAuthorsOfBook(int bookId)
        {
            con.Open();
            List<Author> authors = new List<Author>();
            NpgsqlCommand command = new NpgsqlCommand($@"select author_id, name, surname, birthdate, signature from authors 
            join persons on persons.person_id = authors.author_id
            where authors.author_id in (
                select author_id from linksbooktoauthor
                where linksbooktoauthor.book_id = {bookId})", con);

            var reader = command.ExecuteReader();
            
            while(reader.Read())
            {
                authors.Add(ReadAuthor(reader));
            }

            con.Close();

            return authors;
        }
        #endregion

        #region booksWithReaders
        public bool AddReaderToBook(int readerId, int bookId)
        {
            try
            {
                bool res = false;
                con.Open();

                NpgsqlCommand command = new NpgsqlCommand($"insert into linksreadertobook (book_id, reader_id) values({bookId}, {readerId})", con);

                if (command.ExecuteNonQuery() != 0)
                {
                    res = true;
                }

                con.Close();
                return res;
            }
            catch
            {
                con.Close();
                return false;
            }


        }
        public bool DeleteReaderFromBook(int readerId, int bookId)
        {
            bool res = false;
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"delete from linksreadertobook where book_id = {bookId} and reader_id = {readerId}", con);

            if (command.ExecuteNonQuery() != 0)
            {
                res = true;
            }

            con.Close();
            return res;
        }
        public IEnumerable<Reader> GetReadersOfBook(int bookId)
        {
            con.Open();
            List<Reader> readers = new List<Reader>();
            NpgsqlCommand command = new NpgsqlCommand($@"select reader_id, name, surname, birthdate, favouritegenre from readers 
            join persons on persons.person_id = readers.reader_id
            where readers.reader_id in (
                select reader_id from linksreadertobook
                where linksreadertobook.book_id = {bookId})", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                readers.Add(ReadReader(reader));

            }

            con.Close();

            return readers;
        }
        #endregion

        public IEnumerable<Book> SearchBooks(string title, int minPagesCount, int maxPagesCount)
        {
            List<Book> books = new List<Book>();

            con.Open();

            NpgsqlCommand command = new NpgsqlCommand($"select * from books where title like '%{title}%' and pagescount > {minPagesCount} and pagescount<{maxPagesCount}", con);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                books.Add(ReadBook(reader));
            }

            con.Close();
            return books;
        }
    }
}
