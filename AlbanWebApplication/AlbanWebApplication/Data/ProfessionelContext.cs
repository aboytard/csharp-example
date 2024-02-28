using Microsoft.EntityFrameworkCore;

namespace AlbanWebApplication.Data
{
    public class ProfessionelContext : DbContext
    {
        public ProfessionelContext (DbContextOptions<ProfessionelContext> options)
            : base(options)
        {
        }

        public DbSet<AlbanWebApplication.Models.Professionel> Professionels { get; set; } = default!;
    }
}
