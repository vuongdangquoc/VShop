using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeStatus(int id)
        {
            var post = await _unitOfWork.PostRepository.GetPostById(id);
            if (post != null) 
            {
                post.Status = !post.Status;
                _unitOfWork.PostRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> Create(Post post)
        {
            _unitOfWork.PostRepository.Add(post);
            var result =  await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
           var post = await _unitOfWork.PostRepository.GetPostById(id);
            if (post == null) return false;
            _unitOfWork.PostRepository.Delete(id);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Post>> GetActivePostsAsync()
        {
            return await _unitOfWork.PostRepository.GetActivePosts();
        }

        public async Task<IEnumerable<Post>> GetAllCategoryAsync(string? search, bool? status)
        {
            return await _unitOfWork.PostRepository.GetAllPostAsync(search, status);
        }

        public async Task<List<Post>> GetAllPostAsync()
        {
            return await _unitOfWork.PostRepository.GetAllPostAsync();
        }

        public async Task<PostDetailDTO?> GetPostByIdAsync(int id)
        {
            var post = await _unitOfWork.PostRepository.GetPostById(id);

            if(post != null)
            {
                var postDTO = ConvertToPostDetailDTO(post);
                postDTO.prevId = await _unitOfWork.PostRepository.GetPrevPostId(id);
                postDTO.nextId = await _unitOfWork.PostRepository.GetNextPostId(id);
                return postDTO;
            }
            return null;
        }

        public async Task<bool> Update(Post post)
        {
            var findPost = await _unitOfWork.PostRepository.GetPostById(post.Id);
            if (findPost == null) return false;
            findPost.Title = post.Title;
            findPost.Content = post.Content;
            findPost.Status = post.Status;
            findPost.Image = post.Image;
            _unitOfWork.PostRepository.Update(findPost);
            var result =  await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        private  PostDetailDTO ConvertToPostDetailDTO(Post post)
        {
            var postDetailDTO = new PostDetailDTO()
            {
                Id = post.Id,
                Content = post.Content,
                CreateAt = post.CreateAt,
                CreateBy = post.CreateBy,
                Image = post.Image,
                NameAuthor = post.CreateByNavigation.FullName,
                Status = post.Status,
            };
            return postDetailDTO;
        }
    }
}
