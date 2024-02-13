﻿using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkExample.Data
{
    public class BloggingContext : DbContext
    {
        private string dbPath = "";
        private DatabaseType databaseType;

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext(string dbPath, DatabaseType databaseType)
        {
            this.dbPath = dbPath;
            this.databaseType = databaseType;
        }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (databaseType == DatabaseType.SQLite)
                optionsBuilder.UseSqlite(dbPath);
            else if (databaseType == DatabaseType.SQLServer)
                optionsBuilder.UseSqlServer(dbPath);
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
}
