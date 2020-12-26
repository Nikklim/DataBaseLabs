using DatabaseLab2.Views;
using DataBaseLab3.DAL.Model;
using DataBaseLab3.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLab3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Library library = new Library();
            //AuthorRepository authorRepository = new AuthorRepository(library);
            //authorRepository.CreateOrUpdate(new Author
            //{
            //    Signature = "AA",
            //    Person = new Person
            //    {
            //        Name = "Nikita",
            //        Surname = "Klymchuk"
            //    }
            //});
            MainViewConsole mainView = new MainViewConsole(new DatabaseLab2.Controllers.MainViewController(new Library()));

            mainView.Run();
        }
    }
}
