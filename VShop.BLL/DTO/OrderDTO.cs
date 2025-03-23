using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class OrderDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9][0-9]{8,9})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public string? Note { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public int Status { get; set; }

        public Guid UserId { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Ward { get; set; }

        public int? VoucherId { get; set; }
        [Required]
        public double DeliveryFee { get; set; }
        [Required]
        public int TypePayment { get; set; }
    }
}
