using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class CommentDTO
    {
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int ParentId { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public string Avatar {  get; set; }

        public int ProductId { get; set; }
    }
}
