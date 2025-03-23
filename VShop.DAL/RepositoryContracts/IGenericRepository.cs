using System.Linq.Expressions;

namespace VShop.DAL.RepositoryContracts
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        void AddRange(List<T> list);
        void Add(T entity);
        void Update(T entity);
        IQueryable<T> GetQuery();
    }
}
