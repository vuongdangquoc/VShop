using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly VShopContext _context;
        public CommentRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
