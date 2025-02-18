using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class FavoriteProductRepository : GenericRepository<FavoriteProduct>, IFavoriteProductRepository
    {
        private readonly VShopContext _context;
        public FavoriteProductRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
