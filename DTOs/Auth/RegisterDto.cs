using System.ComponentModel.DataAnnotations;

namespace api_pd.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage = "รหัสผ่านต้องอย่างน้อย 6 ตัว")]
        public string Password { get; set; } = null!;

    }
}
