using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkExample.Data
{
    public class BloggingContext : DbContext
    {
        //private string connectionString = "";
        //private DatabaseType databaseType;

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext(/*string connection, DatabaseType databaseType*/)
        {
            //connectionString = connection;
            //this.databaseType = databaseType;
        }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (databaseType == DatabaseType.SQLite)
            //    optionsBuilder.UseSqlite(connectionString);
            //else if (databaseType == DatabaseType.SQLServer)
            optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=BlogDb;User Id=sa;Password=Test123456789!;MultipleActiveResultSets = true;");
        }
    }

    //public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    //{
    //    public BloggingContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
    //        optionsBuilder.UseSqlite("Data Source=blog.db");

    //        return new BloggingContext(optionsBuilder.Options);
    //    }
    //}

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
