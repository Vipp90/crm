using Microsoft.EntityFrameworkCore;

namespace crm.Models
{
    public class CurrencyContext:DbContext

    {
        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
        {


        }

        public DbSet<Currency> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>()
                .HasIndex(p => new { p.symbol})
                .IsUnique(true);
        }
    }
}
