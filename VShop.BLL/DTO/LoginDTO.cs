using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.Helper;

namespace VShop.BLL.DTO
{
    public class LoginDTO
    {
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        [EmailValidation]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }    
    }
}
