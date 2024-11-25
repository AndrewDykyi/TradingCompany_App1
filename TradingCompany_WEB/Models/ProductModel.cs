using System.ComponentModel.DataAnnotations;

namespace TradingCompany_WEB.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
