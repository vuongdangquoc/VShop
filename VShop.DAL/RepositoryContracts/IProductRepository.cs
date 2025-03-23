using VShop.DAL.Enums;
using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<Product?> GetProductByIdAsync(int id);
        public Product? GetProductById(int id);
        public Task<List<Product>> GetProductsFromListProductIdAsync(List<int> listId);
        public Task<List<Product>> GetNewArrialsAsync(int count,string category ="all");
        public Task<int> GetNumOfProduct();
        Task<IEnumerable<Product>> GetAllProductsAsync(string? search,
           string? category,
           double? minPrice,
           double? maxPrice,
           int page,
           int pageSize,
           SortBy sortBy,
           bool isDescending);

        Task<IEnumerable<Product>> GetAllProductsInAdminAsync(string? search,
           int? categoryId,
           bool? status,
           SortBy sortBy,
           bool isDescending);

        public Task<bool> ReduceQuantityProduct(int idProduct, int count);

        public Task<int?> GetQuantityProductAsync(int idProduct);

        public Task<bool> CheckExistProductAsync(int idProduct);

        public void DeleteProduct(int idProduct);
    }
}
