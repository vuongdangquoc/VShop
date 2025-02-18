using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
