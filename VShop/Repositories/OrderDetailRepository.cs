using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly VShopContext _context;
        public OrderDetailRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
