using Microsoft.EntityFrameworkCore;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly VShopContext _context;

        public OrderRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Order?> GetNewByUserID(Guid userId)
        {
           var order = await _context.Orders.Include(x => x.OrderDetails).Where(x => x.UserId == userId).OrderByDescending(x => x.OrderDate).FirstOrDefaultAsync();
            return order;
        }
    }
}
