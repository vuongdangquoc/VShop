using Microsoft.EntityFrameworkCore;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly VShopContext _context;
        public PostRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<List<Post>> GetActivePosts()
        {
            return await _context.Posts.Where(x => x.Status).OrderByDescending(x => x.CreateAt).ToListAsync();    
        }

        public async Task<List<Post>> GetAllPostAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostAsync(string? search, bool? status)
        {
            var query = _context.Posts.Include(x => x.CreateByNavigation)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
            }

            if (status != null)
            {
                query = query.Where(p => p.Status == status);
            }

            return await query.ToListAsync();
        }

        public async Task<int?> GetNextPostId(int currentId)
        {
            var prev = await _context.Posts.Where(x => x.Status && x.Id > currentId).OrderBy(x => x.CreateAt).FirstOrDefaultAsync();
            if (prev != null)
            {
                return prev.Id;
            }
            return null;
        }

        public async Task<Post?> GetPostById(int id)
        {
            return await _context.Posts.Include(x => x.CreateByNavigation).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int?> GetPrevPostId(int currentId)
        {
            var prev = await _context.Posts.Where(x => x.Status && x.Id < currentId).OrderByDescending(x => x.CreateAt).FirstOrDefaultAsync();
            if(prev != null)
            {
                return prev.Id;
            }
            return null;
        }

        public async Task<bool> IsLatestBlogPost(int id)
        {
            var latestPost =await  _context.Posts
            .OrderByDescending(p => p.CreateAt)
            .FirstOrDefaultAsync();

            return latestPost != null && latestPost.Id == id;
        }

        public async Task<bool> IsOldestBlogPost(int id)
        {
            var oldestPost = await _context.Posts
           .OrderBy(p => p.CreateAt)
           .FirstOrDefaultAsync();

            return oldestPost != null && oldestPost.Id == id;
        }

        public void Delete(int id)
        {
            var p = _context.Posts.SingleOrDefault(x => x.Id == id);
            if (p != null)
                _context.Posts.Remove(p);
        }
    }
}
