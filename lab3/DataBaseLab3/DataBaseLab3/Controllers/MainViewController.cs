using DatabaseLab2.Views;
using DataBaseLab3.DAL.Model;
using DataBaseLab3.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class MainViewController
    {
        public Dictionary<string, Func<MainViewArgs, object>> CommandsHandler { get; private set; }
        private BookRepository booksRepo;
        private ReaderRepository readersRepo;
        private AuthorRepository authorsRepo;
        

        public MainViewController(DbContext dbContext)
        {

            booksRepo = new BookRepository(dbContext);
            readersRepo = new ReaderRepository(dbContext);
            authorsRepo = new AuthorRepository(dbContext);


            CommandsHandler = new Dictionary<string, Func<MainViewArgs, object>>
            {
                #region booksCRUD
                {
                    "get_books",
                    x =>
                        {
                            string res = "";
                            foreach(var book in booksRepo.GetAll())
                            {
                                res += (book.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_book",
                    x =>
                    {
                        return booksRepo.Get(x.BookId).ToString();
                    }
                },
                {
                    "delete_book",
                    x =>
                    {
                        booksRepo.Remove(x.BookId);
                        return okMessage;
                    }
                },
                {
                    "insert_book",
                    x =>
                    {
                        booksRepo.CreateOrUpdate(x.Book);
                        return okMessage;
                    }
                },
                {
                    "update_book",
                    x =>
                    {
                        booksRepo.CreateOrUpdate(x.Book);
                        return okMessage;
                    }

                },
                #endregion
                #region booksAuthors
                {
                    "AddAuthorToBook",
                    x =>
                    {
                        booksRepo.Get(x.BookId).Authors.Add(authorsRepo.Get(x.AuthorId));
                        return okMessage;
                    }
                },
                {
                    "DeleteAuthorFromBook",
                    x =>
                    {
                        booksRepo.Get(x.BookId).Authors.Remove(authorsRepo.Get(x.AuthorId));
                        return okMessage;
                    }
                },
                {
                    "GetAuthorsOfBook",
                    x =>
                    {
                        string res = "";
                        foreach(var author in booksRepo.Get(x.BookId).Authors)
                        {
                            res += (author.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion
                #region booksReaders
                {
                    "AddReaderToBook",
                    x =>
                    {
                        booksRepo.Get(x.BookId).Readers.Add(readersRepo.Get(x.ReaderId));
                        return okMessage;
                    }
                },
                {
                    "DeleteReaderFromBook",
                    x =>
                    {
                        booksRepo.Get(x.BookId).Readers.Remove(readersRepo.Get(x.ReaderId));
                        return okMessage;
                    }
                },
                {
                    "GetReadersOfBook",
                    x =>
                    {
                        string res = "";
                        foreach(var reader in booksRepo.Get(x.BookId).Readers)
                        {
                            res += (reader.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion
                #region authorsCRUD
                {
                    "get_authors",
                    x =>
                        {
                            string res = "";
                            foreach(var author in authorsRepo.GetAll())
                            {
                                res += (author.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_author",
                    x =>
                    {
                        return authorsRepo.Get(x.AuthorId).ToString();
                    }
                },
                {
                    "delete_author",
                    x =>
                    {
                        authorsRepo.Remove(x.AuthorId);
                        return okMessage;
                    }
                },
                {
                    "insert_author",
                    x =>
                    {
                        authorsRepo.CreateOrUpdate(x.Author);
                        return okMessage;
                    }
                },
                {
                    "update_author",
                    x =>
                    {
                        authorsRepo.CreateOrUpdate(x.Author);
                        return okMessage;
                    }

                },
                {
                    "get_authorsbooks",
                    x =>
                    {
                        string res = "";
                        foreach(var book in authorsRepo.Get(x.AuthorId).Books)
                        {
                            res += (book.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion
                #region readersCRUD
                {
                    "get_readers",
                    x =>
                        {
                            string res = "";
                            foreach(var reader in readersRepo.GetAll())
                            {
                                res += (reader.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "get_reader",
                    x =>
                    {
                        return readersRepo.Get(x.ReaderId).ToString();
                    }
                },
                {
                    "delete_reader",
                    x =>
                    {
                        readersRepo.Remove(x.ReaderId);
                        return okMessage;
                    }
                },
                {
                    "insert_reader",
                    x =>
                    {
                        readersRepo.CreateOrUpdate(x.Reader);
                        return okMessage;
                    }
                },
                {
                    "update_reader",
                    x =>
                    {
                        readersRepo.CreateOrUpdate(x.Reader);
                        return okMessage;
                    }

                },
                {
                    "get_readersbooks",
                    x =>
                    {
                        string res = "";
                        foreach(var book in readersRepo.Get(x.ReaderId).Books)
                        {
                            res += (book.ToString() + "\n");
                        }
                        return res;
                    }
                },
                #endregion
                {
                    "add_random_authors",
                    x =>
                    {
                        return GetBooLReturnMessage(authorsRepo.AddRandomAuthorsToDB(x.RandomCount));
                    }
                },
                {
                    "search_books",
                    x =>
                    {
                        string res = "";
                            foreach(var book in booksRepo.SearchBooks(x.SearchBookParameters.title, x.SearchBookParameters.minPagesCount, x.SearchBookParameters.maxPagesCount))
                            {
                                res += (book.ToString() + "\n");
                            }
                            return res;
                    }
                },
                {
                    "search_authors",
                    x =>
                        {
                            string res = "";
                            foreach(var author in authorsRepo.SearchAuthors(x.SearchAuthorParameters.Signature, x.SearchAuthorParameters.Name, x.SearchAuthorParameters.MinDate))
                            {
                                res += (author.ToString() + "\n");
                            }
                            return res;
                       }
                },
                {
                    "search_readers",
                    x =>
                        {
                            string res = "";
                            foreach(var reader in readersRepo.SearchReaders(x.SearchReaderParameters.Favouritegenre, x.SearchReaderParameters.Name, x.SearchReaderParameters.Surname))
                            {
                                res += (reader.ToString() + "\n");
                            }
                            return res;
                       }
                },
            };
        }


        public Reader GetReader(int id)
        {
            return readersRepo.Get(id);
        }
        public Author GetAuthor(int id)
        {
            return authorsRepo.Get(id);
        }
        public Book GetBook(int id)
        {
            return booksRepo.Get(id);
        }
        private string GetBooLReturnMessage(bool b)
        {
            if (b) return "done";
            else return "ERROR";
        }
        private const string okMessage = "OK";
    }
}
