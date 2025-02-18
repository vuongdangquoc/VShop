using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
