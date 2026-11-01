using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Valera> Valeras => Set<Valera>();
    }
}
