using EntityFrameworkExample.Data;

namespace EntityFrameworkExample
{
    public static class DbCreation
    {
        public static void CreateDataBase(string connectionString, DatabaseType databaseType)
        {
            BloggingContext blogginContext;
            switch (databaseType)
            {
                case DatabaseType.SQLite:
                    if (!File.Exists(connectionString))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(connectionString));
                    }
                    //blogginContext = new BloggingContext($"Data Source={connectionString}", databaseType);
                    break;
                case DatabaseType.SQLServer:
                    //blogginContext = new BloggingContext(connectionString, databaseType);
                    break;
                default:
                    throw new NotImplementedException();
            }
            Console.WriteLine($"Creating new DB in {connectionString}");
            try
            {
                //blogginContext.Database.EnsureCreated();
                Console.WriteLine($"Created checked");
                //blogginContext.Blogs.Add(new Blog()
                //{
                //    BlogId = 0,
                //    Url = "url/0",
                //    Posts = new List<Post>()
                //    {
                //        new Post()
                //        {
                //            PostId = 0, Title = "Post/0", Content = "Content/0", BlogId = 0
                //        }
                //    }
                //});
                Console.WriteLine($"Creating new DB in {connectionString}");
                //blogginContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
