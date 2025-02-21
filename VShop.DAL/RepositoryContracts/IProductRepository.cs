using VShop.DAL.Enums;
using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<Product?> GetProductByIdAsync(int id);
        public Task<List<Product>> GetProductsFromListProductIdAsync(List<int> listId);
        public Task<List<Product>> GetNewArrialsAsync(int count,string category ="all");

        Task<IEnumerable<Product>> GetAllProductsAsync(string? search,
           string? category,
           double? minPrice,
           double? maxPrice,
           int page,
           int pageSize,
           SortBy sortBy,
           bool isDescending);
    }
}
