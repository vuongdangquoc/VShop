using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class AddContactDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9][0-9]{8,9})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
