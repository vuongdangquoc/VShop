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
        [Required]
        [StringLength(255,MinimumLength = 5,ErrorMessage = "Name must be between 5 and 100 characters")]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        [EmailValidation]
        [StringLength(255)]
        public string? Email { get; set; }
        [Required]
        [Range(0,9,ErrorMessage ="Please enter just number from 0 ->9")]
        [StringLength(11)]
        public string? Phone { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [ConfirmPasswordValidation("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
