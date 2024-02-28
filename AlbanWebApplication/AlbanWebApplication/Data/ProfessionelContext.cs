using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlbanWebApplication.Models;

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
