using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
    {
        private readonly VShopContext _context;
        public ProvinceRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
