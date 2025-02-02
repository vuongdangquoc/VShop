using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly VShopContext _context;

        public RoleRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
