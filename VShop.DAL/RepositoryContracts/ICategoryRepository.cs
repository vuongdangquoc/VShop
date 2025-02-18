using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<List<string>> GetListNameCategorySold();
        public Task<List<string>> GetListNameCategoryHasProduct();

    }
}
