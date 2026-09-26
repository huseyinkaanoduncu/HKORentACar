using HKORentACar.Core.Utils.GenericRepositoryPattern;
using HKORentACar.Data.Context;

namespace Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HKODbContext _context;

        public UnitOfWork(HKODbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}