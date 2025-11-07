using Microsoft.EntityFrameworkCore;
using web.Models;

namespace web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Valera> Valeras => Set<Valera>();
    }
}
