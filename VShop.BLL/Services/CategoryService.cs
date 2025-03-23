using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
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

        public async Task<List<Category>> GetAllCategory()
        {
            var  temp = await _unitOfWork.CategoryRepository.GetAllAsync();

            return temp.ToList();
        }

        public async Task<bool> Create(Category category)
        {
            _unitOfWork.CategoryRepository.Add(category);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(Category category)
        {
            var cate = await _unitOfWork.CategoryRepository.GetCategoryById(category.Id);
            if (cate == null) return false;
            cate.Name =category.Name;
            cate.Status = category.Status;
            _unitOfWork.CategoryRepository.Update(cate);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Delete(int categoryId)
        {
            _unitOfWork.CategoryRepository.Delete(categoryId);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task ChangeStatus(int id)
        {
            var cate = await _unitOfWork.CategoryRepository.GetCategoryById(id);
            if (cate == null) return;
            cate.Status = !cate.Status;
            _unitOfWork.CategoryRepository.Update(cate);
            var result = await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(string? search, bool? status)
        {
           return await _unitOfWork.CategoryRepository.GetAllCategoryAsync(search, status);
        }
    }
}
