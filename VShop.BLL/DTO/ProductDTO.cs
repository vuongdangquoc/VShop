namespace VShop.BLL.DTO
{ 
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int Quantity { get; set; }
        public string? Image { get; set; }
        public int Discount { get; set; }
        public string? CurrentPrice { get; set; }
        public List<string>? ListImages { get; set; }
        public string? OldPrice { get; set; }
        public string? Describe { get; set; }
        public string? CategoryName { get; set; }

    }
}
