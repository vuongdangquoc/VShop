using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.DTO
{
    public class ProductCartDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }

        public int Count { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }

        public double? SumPrice { get; set; }
    }
}
