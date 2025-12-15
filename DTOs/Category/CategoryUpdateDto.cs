using System.ComponentModel.DataAnnotations;

namespace api_pd.DTOs.Category
{
    public class CategoryUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

    }
}
