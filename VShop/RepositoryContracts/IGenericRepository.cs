using System.Linq.Expressions;

namespace VShop.RepositoryContracts
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllCategoryAsync();
        void AddRangeAsync(List<T> list);
        void Add(T entity);
        void Update(T entity);
        IQueryable<T> GetQuery();
    }
}
