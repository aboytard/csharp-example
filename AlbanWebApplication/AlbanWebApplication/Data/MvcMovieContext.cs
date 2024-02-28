using Microsoft.EntityFrameworkCore;

namespace AlbanWebApplication.Data
{
    // name has been changed : can be changed back to the MvcMovieContext
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<AlbanWebApplication.Models.Movie> Movie { get; set; } = default!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(dbPath);
        //}
    }
}
