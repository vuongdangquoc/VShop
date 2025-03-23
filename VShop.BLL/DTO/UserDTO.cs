using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }


        public string? UserName { get; set; }

        public string? Image { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }
    }
}
