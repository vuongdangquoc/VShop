using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
