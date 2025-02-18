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
    }
}
