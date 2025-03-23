using VShop.DAL.Enums;
using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<List<string>> GetListNameCategorySold();
        public Task<List<string>> GetListNameCategoryHasProduct();
        public Task<Category?> GetCategoryById(int id);

        Task<IEnumerable<Category>> GetAllCategoryAsync(string? search,bool? status);
        public void Delete(int id);
    }
}
