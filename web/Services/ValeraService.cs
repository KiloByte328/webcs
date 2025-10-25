using web.Data;
using web.Models;

namespace web.Service
{
    public class ValeraService
    {
        private readonly AppDbContext _context;
        public IEnumerable<Valera> GetAllValeras()
        {
            return _context.Valeras.ToList();
        }
        public Valera? GetValeraById(int id)
        {
            return _context.Valeras.FirstOrDefault(v => v.Id == id);
        }
        public void AddValeraToDb(Valera valera)
        {
            _context.Valeras.Add(valera);
            _context.SaveChanges();
        }
    }
}
