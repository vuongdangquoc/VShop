using Microsoft.EntityFrameworkCore;
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

        public async Task<List<string>> GetListNameCategoryHasProduct()
        {
            var lisCategoryName = await _context.Products.Include(x => x.Category)
                .Select(x => x.Category.Name).Distinct().ToListAsync();
            return lisCategoryName;

        }

        public async Task<List<string>> GetListNameCategorySold()
        {
            var listProductIdSold = _context.OrderDetails.Select(x => x.ProductId).Distinct();
            var lisCategoryName = await _context.Products.Include(x => x.Category)
                .Where(x => listProductIdSold.Contains(x.Id))
                .Select(x => x.Category.Name).Distinct().ToListAsync();
            return lisCategoryName;
        }
    }
}
