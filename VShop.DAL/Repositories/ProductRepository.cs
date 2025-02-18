using Microsoft.EntityFrameworkCore;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly VShopContext _context;

        public ProductRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<List<Product>> GetProductsFromListProductIdAsync(List<int> listIds)
        {
            var products = await _context.Products.Where(x=>x.Status).Where(x => listIds.Contains(x.Id)).ToListAsync();
            return products;
        }

        public async Task<List<Product>> GetNewArrialsAsync(int count, string category = "all")
        {
            if(category == "all")
            {
                return await _context.Products.OrderByDescending(p => p.CreatedDate).Take(count).ToListAsync();
            }
            var result = await _context.Products.Where(x => x.Status).Include(x => x.Category).Where(x => x.Category.Name == category).OrderByDescending(p => p.CreatedDate).Take(count).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(
      string? search,
      string? category,
      double? minPrice,
      double? maxPrice,
      int page,
      int pageSize,
      SortBy sortBy,
      bool isDescending)
        {
            var query = _context.Products
                                .Include(p => p.Category)
                                .Where(p => p.Status)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.Contains(category));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = sortBy switch
            {
                SortBy.Price => isDescending ? query.OrderByDescending(p => (p.Price * (100 - p.Discount) / 100)) : query.OrderBy(p => (p.Price * (100 - p.Discount) / 100)),
                SortBy.Name => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                SortBy.Date => isDescending ? query.OrderByDescending(p => p.CreatedDate): query.OrderBy(p => p.CreatedDate),
                _ => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            };

            return await query.Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();
        }
    }
}
