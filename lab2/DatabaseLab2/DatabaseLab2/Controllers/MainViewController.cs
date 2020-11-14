using DatabaseLab2.Configuration;
using DatabaseLab2.Models.DataBaseModels;
using DatabaseLab2.Models.Repositories;
using DatabaseLab2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Controllers
{
    class MainViewController
    {
        public Dictionary<string, Func<MainViewArgs, object>> CommandsHandler { get; private set; }
        private BooksRepository booksRepo;
        private ReadersRepository readersRepo;
        private AuthorsRepository authorsRepo;
        

        public MainViewController(string connectionString)
        {
            booksRepo = new BooksRepository(connectionString);
            readersRepo = new ReadersRepository(connectionString);
            authorsRepo = new AuthorsRepository(connectionString);


            CommandsHandler = new Dictionary<string, Func<MainViewArgs, object>>
            {
                #region booksCRUD
                {
                    "get_books",
                    x =>
                        {
                            string res = "";
                            foreach(var book in booksRepo.GetAllBooks())
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
                        return booksRepo.GetBook(x.BookId).ToString();
                    }
                },
                {
                    "delete_book",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.DeleteBook(x.BookId));
                    }
                },
                {
                    "insert_book",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.InsertBook(x.Book));
                    }
                },
                {
                    "update_book",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.UpdateBook(x.Book));
                    }

                },
                #endregion
                #region booksAuthors
                {
                    "AddAuthorToBook",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.AddAuthorToBook(x.AuthorId, x.BookId));
                    }
                },
                {
                    "DeleteAuthorFromBook",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.DeleteAuthorFromBook(x.AuthorId, x.BookId));
                    }
                },
                {
                    "GetAuthorsOfBook",
                    x =>
                    {
                        string res = "";
                        foreach(var author in booksRepo.GetAuthorsOfBook(x.BookId))
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
                        return GetBooLReturnMessage(booksRepo.AddReaderToBook(x.ReaderId, x.BookId));
                    }
                },
                {
                    "DeleteReaderFromBook",
                    x =>
                    {
                        return GetBooLReturnMessage(booksRepo.DeleteReaderFromBook(x.ReaderId, x.BookId));
                    }
                },
                {
                    "GetReadersOfBook",
                    x =>
                    {
                        string res = "";
                        foreach(var reader in booksRepo.GetReadersOfBook(x.BookId))
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
                            foreach(var author in authorsRepo.GetAllAuthors())
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
                        return authorsRepo.GetAuthor(x.AuthorId).ToString();
                    }
                },
                {
                    "delete_author",
                    x =>
                    {
                        return GetBooLReturnMessage(authorsRepo.DeleteAuthor(x.AuthorId));
                    }
                },
                {
                    "insert_author",
                    x =>
                    {
                        return GetBooLReturnMessage(authorsRepo.InsertAuthor(x.Author));
                    }
                },
                {
                    "update_author",
                    x =>
                    {
                        return GetBooLReturnMessage(authorsRepo.UpdateAuthor(x.Author));
                    }

                },
                {
                    "get_authorsbooks",
                    x =>
                    {
                        string res = "";
                        foreach(var book in authorsRepo.GetAuthorsBooks(x.AuthorId))
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
                            foreach(var reader in readersRepo.GetAllReaders())
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
                        return readersRepo.GetReader(x.ReaderId).ToString();
                    }
                },
                {
                    "delete_reader",
                    x =>
                    {
                        return GetBooLReturnMessage(readersRepo.DeleteReader(x.ReaderId));
                    }
                },
                {
                    "insert_reader",
                    x =>
                    {
                        return GetBooLReturnMessage(readersRepo.InsertReader(x.Reader));
                    }
                },
                {
                    "update_reader",
                    x =>
                    {
                        return GetBooLReturnMessage(readersRepo.UpdateReader(x.Reader));
                    }

                },
                {
                    "get_readersbooks",
                    x =>
                    {
                        string res = "";
                        foreach(var book in readersRepo.GetReadersBooks(x.ReaderId))
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
            return readersRepo.GetReader(id);
        }
        public Author GetAuthor(int id)
        {
            return authorsRepo.GetAuthor(id);
        }
        public Book GetBook(int id)
        {
            return booksRepo.GetBook(id);
        }
        private string GetBooLReturnMessage(bool b)
        {
            if (b) return "done";
            else return "ERROR";
        }
    }
}
