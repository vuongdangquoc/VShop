using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using VShop.DAL.RepositoryContracts;
using VShop.DAL.Models.Db;

namespace VShop.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly VShopContext _db;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(VShopContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRangeAsync(List<T> list)
        {
            _dbSet.AddRangeAsync(list);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> GetQuery()
        {
            return _dbSet;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
