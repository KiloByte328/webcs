using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Service
{
    public class ValeraService
    {
        private readonly AppDbContext _context;
        public ValeraService(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IEnumerable<Valera>> GetAllValeras()
        {
            
            return  await _context.Valeras.ToListAsync();
        }
        public async Task<Valera?> GetValeraById(int id)
        {
            return await _context.Valeras.FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task AddValeraToDb(Valera valera)
        {
            await _context.Valeras.AddAsync(valera);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateValeraInDb(Valera valera)
        {
            _context.Valeras.Update(valera);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteValera(Valera valera)
        {
            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();
        }
    }
}
