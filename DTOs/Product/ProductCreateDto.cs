using System.ComponentModel.DataAnnotations;

namespace api_pd.DTOs.Product
{
    public class ProductCreateDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
