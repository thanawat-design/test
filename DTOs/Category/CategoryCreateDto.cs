using System.ComponentModel.DataAnnotations;

namespace api_pd.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(255)]
        public string? Description { get; set; }


    }
}
