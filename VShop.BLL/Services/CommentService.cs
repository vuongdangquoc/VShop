using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CommentDTO>> GetAllCommentOfProduct(int productId)
        {
            var comments = await _unitOfWork.CommentRepository.GetCommentsByProductId(productId);
            var result = comments.Select(x => new CommentDTO
            {
                Content = x.Content,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                ParentId = x.ParentId,
                ProductId = productId,
                UserId = x.UserId,
                UserName = x.User.UserName,
                Avatar = x.User.Image
            });
            return result.ToList();
        }
    }
}
