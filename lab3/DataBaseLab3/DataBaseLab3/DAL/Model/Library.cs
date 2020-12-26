using Npgsql;
using System;
using System.Data.Entity;
using System.Linq;

namespace DataBaseLab3.DAL.Model
{
    class Library : DbContext
    {
        // Контекст настроен для использования строки подключения "Library" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "DataBaseLab3.Library" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "Library" 
        // в файле конфигурации приложения.
        public Library()
            : base("name=Library")
        {
            Database.SetInitializer(new DropCreateDb());
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>();
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Reader> Readers { get; set; }
        public virtual DbSet<Book> Books { get; set; }
    }

    class DropCreateDb : DropCreateDatabaseIfModelChanges<Library>
    {
        protected override void Seed(Library context)
        {
            context.Database.ExecuteSqlCommandAsync("CREATE INDEX ix_Signature ON dbo.\"Authors\" USING btree (\"Signature\");").Wait();
            context.Database.ExecuteSqlCommandAsync("CREATE INDEX ix_Name ON dbo.\"Persons\" USING BRIN (\"Name\");").Wait();
            base.Seed(context);
        }
    }

}