using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly VShopContext _context;

        public OrderRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
