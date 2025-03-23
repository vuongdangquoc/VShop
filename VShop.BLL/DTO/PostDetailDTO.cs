using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class PostDetailDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public Guid CreateBy { get; set; }

        public string NameAuthor { get; set; }

        public int? prevId { get; set; }

        public int? nextId { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
    }
}
