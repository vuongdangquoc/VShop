using VShop.BLL.DTO;

namespace VShop.BLL.ServiceContracts
{
    public interface ICategoryService
    {
        public Task<List<string>> GetListNameCategorySoldAsync();
        public Task<List<string>> GetListNameCategoryHasProductAsync();

        public Task<List<CategoryDTO>?> GetCategoryViewModelsAsync();
    }
}
