using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly VShopContext _context;

        public UserRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
