using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Comment>> GetCommentsByProductId(int id)
        {
            var comments = await _context.Comments.Include(x => x.User).Where(x => x.ProductId == id).ToListAsync();
            return comments;
        }
    }
}
