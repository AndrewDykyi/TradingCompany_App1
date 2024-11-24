namespace DAL.Concrete
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryID { get; set; }
        public decimal Price { get; set; }
        public bool Blocked { get; set; }
    }
}