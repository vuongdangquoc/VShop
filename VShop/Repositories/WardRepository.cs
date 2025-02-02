using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class WardRepository : GenericRepository<Ward>, IWardRepository
    {
        private readonly VShopContext _context;

        public WardRepository(VShopContext db) : base(db)
        {
            _context = db;

        }
    }
}
