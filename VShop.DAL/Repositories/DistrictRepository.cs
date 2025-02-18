using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
