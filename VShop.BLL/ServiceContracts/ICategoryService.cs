using VShop.BLL.DTO;
using VShop.DAL.Models.Db;

namespace VShop.BLL.ServiceContracts
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAllCategory();
        public Task<List<string>> GetListNameCategorySoldAsync();
        public Task<List<string>> GetListNameCategoryHasProductAsync();
        public Task<List<CategoryDTO>?> GetCategoryViewModelsAsync();
        public Task<bool> Create(Category category);
        public Task<bool> Update(Category category);
        public Task<bool> Delete(int categoryId);
        Task ChangeStatus(int id);

        Task<IEnumerable<Category>> GetAllCategoryAsync(string? search, bool? status);
    }
}
