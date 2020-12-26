using DatabaseLab2.Controllers;
using DataBaseLab3.DAL.Model;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLab2.Views
{
    class MainViewConsole
    {
        MainViewController mainController;

        public MainViewConsole(MainViewController controller)
        {
            this.mainController = controller;
            viewCommands = new Dictionary<string, Func<MainViewArgs>>
            {
                #region booksCRUD
                {
                    "get_books",
                    () => new MainViewArgs()
                },
                {
                    "get_book",
                    () => new MainViewArgs{BookId = GetId()}
                },
                {
                    "delete_book",
                    () => new MainViewArgs{BookId = GetId()}
                },
                {
                    "insert_book",
                    () =>
                    {
                        Book book = GetBookValues(null);
                        return new MainViewArgs{ Book = book};
                    }
                },
                {
                    "update_book",
                    () =>
                    {
                        Book book = mainController.GetBook(GetId());
                        if(book == null) throw new Exception();
                        Book book1 = GetBookValues(book);
                        return new MainViewArgs{Book = book1};
                    }
                },
                #endregion

                #region booksAuthors
                {
                    "AddAuthorToBook",
                    () =>
                    {
                        return new MainViewArgs{BookId = GetId("book"), AuthorId = GetId("author")};
                    }
                },
                {
                    "DeleteAuthorFromBook",
                    () =>
                    {
                        return new MainViewArgs{BookId = GetId()};
                    }
                },
                {
                    "GetAuthorsOfBook",
                    () => new MainViewArgs{ BookId = GetId("book")}
                },
                #endregion
                #region booksReaders
                {
                    "AddReaderToBook",
                    () =>
                    {
                        return new MainViewArgs{BookId = GetId("book"), ReaderId = GetId("reader")};
                    }
                },
                {
                    "DeleteReaderFromBook",
                    () =>
                    {
                        return new MainViewArgs{BookId = GetId("book"), ReaderId = GetId("reader")};
                    }
                },
                {
                    "GetReadersOfBook",
                    () => new MainViewArgs{ BookId = GetId("book")}
                },
                #endregion
                #region authorsCRUD
                {
                    "get_authors",
                    () => new MainViewArgs()
                },
                {
                    "get_author",
                    () => new MainViewArgs{AuthorId = GetId()}
                },
                {
                    "delete_author",
                    () => new MainViewArgs{AuthorId = GetId()}
                },
                {
                    "insert_author",
                    () =>
                    {
                        Author author = GetAuthorValues(null);
                        return new MainViewArgs{ Author = author};
                    }
                },
                {
                    "update_author",
                    () =>
                    {
                        Author author = mainController.GetAuthor(GetId());
                        if(author == null) throw new Exception();
                        Author author1 = GetAuthorValues(author);
                        return new MainViewArgs{Author = author1};
                    }
                },
                {
                    "get_authorsbooks",
                    () => new MainViewArgs{ AuthorId = GetId()}
                },
                #endregion
                #region readersCRUD
                {
                    "get_readers",
                    () => new MainViewArgs()
                },
                {
                    "get_reader",
                    () => new MainViewArgs{ReaderId = GetId()}
                },
                {
                    "delete_reader",
                    () => new MainViewArgs{ReaderId = GetId()}
                },
                {
                    "insert_reader",
                    () =>
                    {
                        Reader reader = GetReaderValues(null);
                        return new MainViewArgs{ Reader = reader};
                    }
                },
                {
                    "update_reader",
                    () =>
                    {
                        Reader reader = mainController.GetReader(GetId());
                        if(reader == null) throw new Exception();
                        Reader reader1 = GetReaderValues(reader);
                        return new MainViewArgs{Reader = reader1};
                    }
                },
                {
                    "get_readersbooks",
                    () => new MainViewArgs{ ReaderId = GetId()}
                },
                #endregion
                {
                    "add_random_authors",
                    () => new MainViewArgs{ RandomCount = GetCount()}
                },
                {
                    "search_books",
                    () => new MainViewArgs
                    {
                        SearchBookParameters = new SearchBookParameters
                        {
                            title = GetStr("Enter title:"),
                            minPagesCount = GetInt("Enter minPagesCount:"),
                            maxPagesCount = GetInt("Enter maxPagesCount:")
                        }
                    }
                },
                {
                    "search_readers",
                    () => new MainViewArgs
                    {
                        SearchReaderParameters = new SearchReaderParameters
                        {
                             Favouritegenre = GetStr("Enter favourite genre:"),
                             Name = GetStr("Enter name :"),
                             Surname = GetStr("Enter surname :")
                        }
                    }
                },
                {
                    "search_authors",
                    () => new MainViewArgs
                    {
                        SearchAuthorParameters = new SearchAuthorParameters
                        {
                             Name = GetStr("Enter name :"),
                             Signature = GetStr("Enter signature :"),
                             MinDate = GetDate("Enter min date:")
                        }
                    }
                },
            };
        }
        private string GetStr(string msg)
        {
            Console.WriteLine(msg);
            return Console.ReadLine();
        }
        private int GetInt(string msg)
        {
            Console.WriteLine(msg);
            return Convert.ToInt32(Console.ReadLine());
        }
        private NpgsqlDate GetDate(string msg)
        {
            Console.WriteLine(msg);
            string res = Console.ReadLine();
            return NpgsqlDate.Parse(res);
        }
        private int GetCount()
        {
            Console.Write($"Enter count of authors:");
            return Convert.ToInt32(Console.ReadLine());
        }
        private Reader GetReaderValues(Reader reader)
        {
            Reader res = new Reader();
            if (reader != null)
            {
                Console.WriteLine("Current author");
                Console.WriteLine(reader);
                res.ReaderId = reader.ReaderId;
            }

            Console.WriteLine("Enter favouritegenre:");
            res.FavouriteGenre = Console.ReadLine();

            res.Person = new Person();
            Console.WriteLine("Enter name:");
            res.Person.Name = Console.ReadLine();

            Console.WriteLine("Enter surname:");
            res.Person.Surname = Console.ReadLine();

            Console.WriteLine("Enter birthdate(or null if u don't want to enter this value)");
            string birthdate = Console.ReadLine();
            if (birthdate == "null")
            {
                res.Person.BirthdayDate = null;
            }
            else
            {
                res.Person.BirthdayDate = (DateTime?)NpgsqlDate.Parse(birthdate);
            }

            return res;
        }

        private int GetId(string message = "")
        {
            Console.Write($"Enter {message} id:");
            return Convert.ToInt32(Console.ReadLine());
        }
        private Book GetBookValues(Book book)
        {
            Book res = new Book();
            if(book != null)
            {
                Console.WriteLine("Current book");
                Console.WriteLine(book);
                res.BookId = book.BookId;
            }

            Console.WriteLine("Enter title:");
            res.Title = Console.ReadLine();

            Console.WriteLine("Enter pagescount:");
            res.PagesCount = Convert.ToInt32(Console.ReadLine());

            return res;
        }
        private Author GetAuthorValues(Author author)
        {
            Author res = new Author();
            if (author != null)
            {
                Console.WriteLine("Current author");
                Console.WriteLine(author);
                res.AuthorId = author.AuthorId;
            }

            Console.WriteLine("Enter signature:");
            res.Signature = Console.ReadLine();

            res.Person = new Person();
            Console.WriteLine("Enter name:");
            res.Person.Name = Console.ReadLine();

            Console.WriteLine("Enter surname:");
            res.Person.Surname = Console.ReadLine();

            Console.WriteLine("Enter birthdate(or null if u don't want to enter this value)");
            string birthdate = Console.ReadLine();
            if(birthdate == "null")
            {
                res.Person.BirthdayDate = null;
            }
            else
            {
                res.Person.BirthdayDate = (DateTime?)NpgsqlDate.Parse(birthdate);
            }

            return res;
        }

        private void WriteCommands()
        {
            Console.WriteLine("Commands list:");
            foreach(var str in mainController.CommandsHandler.Keys.ToList())
            {
                Console.WriteLine(str);
            }
        }
        private Dictionary<string, Func<MainViewArgs>> viewCommands;
        public void Run()
        {
            do
            {
                WriteCommands();
                Console.WriteLine("\n");
                Console.WriteLine("Enter command:");
                var command = Console.ReadLine();
                try
                {
                    Console.WriteLine(mainController.CommandsHandler[command](viewCommands[command]()));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("bad value");
                }
                

                Console.WriteLine("\nPress any key");
                Console.ReadKey();
                Console.WriteLine("\n");
            } while (true);
        }





    }
}
