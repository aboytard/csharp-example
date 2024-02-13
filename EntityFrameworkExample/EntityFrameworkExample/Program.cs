using EntityFrameworkExample.Data;

namespace EntityFrameworkExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string dbPath = "C:\\Test - Sample\\EntityFramework\\data.db";
            //var dataBaseChoice = DatabaseType.SQLite;

            var dataBaseChoice = DatabaseType.SQLServer;
            string dbPath = "Server=127.0.0.1;Database=BlogDb;User Id=sa;Password=Test123456789!;MultipleActiveResultSets = true;";

            //var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddDbContext<BloggingContext>(
            //    options => options.UseSqlServer(dbPath));

            //DbCreation.CreateDataBase(dbPath, dataBaseChoice);
            Console.WriteLine("Hello World!");

            DbCreation.ExampleAddFirstDataInSQLServer(dbPath, dataBaseChoice);
        }
    }
}




