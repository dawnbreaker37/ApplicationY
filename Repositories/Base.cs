using ApplicationY.Data;
using ApplicationY.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class Base<T> : IBase<T> where T : class
    {
        private readonly Context _context;
        public Base(Context context)
        {
            _context = context;
        }

        public async Task<int> GetCountByIdAsync(int Id)
        {
            if (Id != 0) return await _context.Set<T>().CountAsync();
            else return 0;
        }
    }
}
