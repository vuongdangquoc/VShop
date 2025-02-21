using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;

namespace VShop.BLL.ServiceContracts
{
    public interface ICommentService
    {
        public Task<List<CommentDTO>> GetAllCommentOfProduct(int productId);
    }
}
