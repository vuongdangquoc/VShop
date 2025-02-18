using Microsoft.EntityFrameworkCore;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly VShopContext _context;
        public OrderDetailRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public Task<List<int>> GetListBestSellingProductIdAsync(int count, string category = "all")
        {
            //Create temp as queryable to store and can be apply filter conditions later
            var temp = _context.OrderDetails.AsQueryable();
            
            // check if category != all
            if (!category.Equals("all"))
            {
                var listProductIdOnCategory = _context.Products.Include(p => p.Category).Where(x => x.Category.Name.Equals(category)).Select(x => x.Id);
                temp = temp.Where(x => listProductIdOnCategory.Contains(x.ProductId));

            }
             
            //get id of best selling products
            var listProductId = temp
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    TotalSold = x.Sum(q => q.Quantity)
                })
                .OrderBy(x => x.ProductId)
                .Take(count)
                .Select(x => x.ProductId)
                .ToListAsync();
            return listProductId;
        }
    }
}
