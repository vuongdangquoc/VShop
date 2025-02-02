using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly VShopContext _context;
        public PostRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
