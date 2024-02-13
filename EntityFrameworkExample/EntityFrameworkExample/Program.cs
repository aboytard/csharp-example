using EntityFrameworkExample.Data;

namespace EntityFrameworkExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string dbPath = "C:\\Test - Sample\\EntityFramework\\data.db";
            //var dataBaseChoice = DatabaseType.SQLite;

            //var dataBaseChoice = DatabaseType.SQLServer;

            //var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddDbContext<BloggingContext>(
            //    options => options.UseSqlServer(dbPath));

            //DbCreation.CreateDataBase(dbPath, dataBaseChoice);
            Console.WriteLine("Hello World!");

            //ExampleAddFirstDataInSQLServer();
        }

        public static void ExampleAddFirstDataInSQLServer()
        {
            using (var blogginContext = new BloggingContext())
            {
                blogginContext.Database.EnsureCreated();
                blogginContext.Blogs.Add(new Blog()
                {
                    BlogId = 0,
                    Url = "url/0",
                    Posts = new List<Post>()
                    {
                        new Post()
                        {
                            PostId = 0, Title = "Post/0", Content = "Content/0", BlogId = 0
                        }
                    }
                });
                blogginContext.SaveChanges();
            }
            Console.WriteLine("Data Added");
            Console.ReadKey();
        }
    }
}




