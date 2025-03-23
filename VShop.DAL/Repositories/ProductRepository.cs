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
            var products = await _context.Products.Include(x => x.Category).Where(x=>x.Status && x.Category.Status).Where(x => listIds.Contains(x.Id)).ToListAsync();
            return products;
        }

        public async Task<List<Product>> GetNewArrialsAsync(int count, string category = "all")
        {
            if(category == "all")
            {
                return await _context.Products.Include(x => x.Category).Where(x => x.Category.Status).OrderByDescending(p => p.CreatedDate).Take(count).ToListAsync();
            }
            var result = await _context.Products.Where(x => x.Status && x.Category.Status).Include(x => x.Category).Where(x => x.Category.Name == category).OrderByDescending(p => p.CreatedDate).Take(count).ToListAsync();
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
                                .Where(x => x.Category.Status)
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

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var result = await _context.Products.Include(x => x.Category).Where(x => x.Category.Status).SingleOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task<bool> ReduceQuantityProduct(int idProduct, int count)
        {
            var product = await _context.Products.SingleOrDefaultAsync(v => v.Id == idProduct);

            if (product != null)
            {
                product.Quantity -= count;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int?> GetQuantityProductAsync(int idProduct)
        {
            try
            {
                var product  = await _context.Products.FirstOrDefaultAsync(x => x.Id == idProduct);
                return product.Quantity;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<bool> CheckExistProductAsync(int idProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == idProduct);
            if(product == null || product.Quantity == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Product? GetProductById(int id)
        {
            var result =  _context.Products.Include(x => x.Category).SingleOrDefault(p => p.Id == id);
            return result;
        }

        public void DeleteProduct(int idProduct)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == idProduct);
            if(product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsInAdminAsync(string? search, int? categoryId, bool? status, SortBy sortBy, bool isDescending)
        {
            var query = _context.Products
                                .Include(p => p.Category)
                                .Where(x => x.Category.Status)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            if (categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if(status != null)
            {
                query = query.Where(p => p.Status == status);
            }

            query = sortBy switch
            {
                SortBy.Price => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                SortBy.Name => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                SortBy.Date => isDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate),
                _ => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            };

            return await query.ToListAsync();

        }

        public async Task<int> GetNumOfProduct()
        {
            return await _context.Products.CountAsync();
        }
    }
}
