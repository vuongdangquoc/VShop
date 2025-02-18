namespace VShop.BLL.DTO
{ 
    public class ProductDTO
    {
        public string? Name { get; set; }

        public string? Image { get; set; }

        public int Discount { get; set; }
        public string? CurrentPrice { get; set; }

        public string? OldPrice { get; set; }
        public string Describe { get; set; }

    }
}
