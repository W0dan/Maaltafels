using Lalena.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lalena.Database
{
    public class MaaltafelsDbContext : DbContext
    {
        public MaaltafelsDbContext(DbContextOptions<MaaltafelsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fout>().ToTable("Fouten");
            modelBuilder.Entity<Resultaat>().ToTable("Resultaten")
                .HasMany(resultaat => resultaat.Fouten);

            base.OnModelCreating(modelBuilder);
        }
    }
}