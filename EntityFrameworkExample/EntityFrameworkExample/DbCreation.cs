using EntityFrameworkExample.Data;

namespace EntityFrameworkExample
{
    public static class DbCreation
    {
        public static void ExampleAddFirstDataInSQLServer(string connectionString, DatabaseType databaseType)
        {
            try
            {
                string dbPath = connectionString;
                switch (databaseType)
                {
                    case DatabaseType.SQLite:
                        if (!File.Exists(connectionString))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(connectionString));
                        }
                        dbPath =  $"Data Source={connectionString}";
                        break;
                    case DatabaseType.SQLServer:
                        //blogginContext = new BloggingContext(connectionString, databaseType);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                var blogginContext = new BloggingContext(dbPath, databaseType);
                using (blogginContext)
                {
                    blogginContext.Database.EnsureCreated();
                    // can only add if the db is empty.. primary key cannot be added twice
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public static void CreateAndAddDataSQLServer()
        {

        }

        public static void CreateDataBase(string connectionString, DatabaseType databaseType)
        {
            BloggingContext blogginContext;

            Console.WriteLine($"Creating new DB in {connectionString}");
        }
    }
}
