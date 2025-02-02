using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        private readonly VShopContext _context;

        public DistrictRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
