using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly VShopContext _context;

        public ProductRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
