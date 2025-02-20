using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.Helper;

namespace VShop.BLL.DTO
{
    public class RegisterDTO
    {
        [StringLength(255,ErrorMessage = "Name must be under 255 characters")]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        [EmailValidation]
        [StringLength(255)]
        public string? Email { get; set; }
        [Required]
        [StringLength(11)]
        public string? Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one number, and one special character.")]
        public string? Password { get; set; }
        [Required]
        [ConfirmPasswordValidation("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
