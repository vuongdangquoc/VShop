using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
