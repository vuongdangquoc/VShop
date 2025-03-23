using Microsoft.EntityFrameworkCore;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly VShopContext _context;
        public CategoryRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public void Delete(int id)
        {
            var cate = _context.Categories.SingleOrDefault(x => x.Id == id);
            if(cate != null)
            _context.Categories.Remove(cate);
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(string? search, bool? status)
        {
            var query = _context.Categories
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (status != null)
            {
                query = query.Where(p => p.Status == status);
            }

            return await query.ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public async Task<List<string>> GetListNameCategoryHasProduct()
        {
            var lisCategoryName = await _context.Products.Include(x => x.Category).Where(x => x.Category.Status)
                .Select(x => x.Category.Name).Distinct().ToListAsync();
            return lisCategoryName;

        }

        public async Task<List<string>> GetListNameCategorySold()
        {
            var listProductIdSold = _context.OrderDetails.Select(x => x.ProductId).Distinct();
            var lisCategoryName = await _context.Products.Include(x => x.Category)
                .Where(x => listProductIdSold.Contains(x.Id) && x.Category.Status)
                .Select(x => x.Category.Name).Distinct().ToListAsync();
            return lisCategoryName;
        }
    }
}
