using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
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
