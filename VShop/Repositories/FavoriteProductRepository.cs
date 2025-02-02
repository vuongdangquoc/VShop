using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
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
