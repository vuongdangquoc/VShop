using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.DAL.Models.Db;

namespace VShop.BLL.ServiceContracts
{
    public interface IPostService
    {
        Task<PostDetailDTO?> GetPostByIdAsync(int id);
        Task<List<Post>> GetAllPostAsync();
        Task<List<Post>> GetActivePostsAsync();

        public Task<bool> Create(Post post);
        public Task<bool> Update(Post post);
        public Task<bool> Delete(int id);
        Task ChangeStatus(int id);

        Task<IEnumerable<Post>> GetAllCategoryAsync(string? search, bool? status);
    }
}
