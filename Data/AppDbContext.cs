using Microsoft.EntityFrameworkCore;
using LinqExampleApp.Models;

namespace LinqExampleApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
