using Application.Repositories.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TMSDbContext _dbContext;
        public GenericRepository(TMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(int id)
        {
            var obj = GetById(id);
            if (obj != null)
            {
                _dbContext.Set<T>().Remove(obj);
            }
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
