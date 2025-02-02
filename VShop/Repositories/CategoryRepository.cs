using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly VShopContext _context;
        public CategoryRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
