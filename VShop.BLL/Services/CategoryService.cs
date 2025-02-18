using Microsoft.EntityFrameworkCore;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public Task<List<string>> GetListNameCategoryHasProductAsync()
        {
            return _unitOfWork.CategoryRepository.GetListNameCategoryHasProduct();
        }

        public Task<List<string>> GetListNameCategorySoldAsync()
        {
            return _unitOfWork.CategoryRepository.GetListNameCategorySold();
        }

        public async Task<List<CategoryDTO>?> GetCategoryViewModelsAsync()
        {
            var list = await _unitOfWork.ProductRepository.GetQuery()
                .Include(x => x.Category)
                .GroupBy(x => x.Category.Name)
                .Select(g => new CategoryDTO
                {
                    Name = g.Key,
                    Count = g.Count()
                }).ToListAsync();
            return list;
        }
    }
}
