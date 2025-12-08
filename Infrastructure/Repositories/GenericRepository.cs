using Application.Repositories.Interfaces;
using Infrastructure.Context;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TMSDbContext _dbContext;
        public GenericRepository(TMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task Insert(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task Delete(int id)
        {
            var obj = await GetById(id);
            if (obj != null)
            {
                _dbContext.Set<T>().Remove(obj);
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
