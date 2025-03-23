using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostAsync();
        Task<List<Post>> GetActivePosts();

        Task<IEnumerable<Post>> GetAllPostAsync(string? search, bool? status);
        Task<Post?> GetPostById(int id);
        Task<int?> GetPrevPostId(int currentId);
        void Delete(int id);
        Task<int?> GetNextPostId(int currentId);
    }
}
