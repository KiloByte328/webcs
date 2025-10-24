using Microsoft.EntityFrameworkCore;
using web.Models;

namespace web.Data
{
    public class AppDbContext : DbContext
    {
        DbSet<Valera> Valeras => Set<Valera>();
    }
}
