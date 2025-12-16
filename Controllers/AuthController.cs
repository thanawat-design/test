using api_pd.Data;
using api_pd.DTOs.Auth;
using api_pd.Interfaces;
using api_pd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace api_pd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _service.RegisterAsync(dto);
            return Ok("สมัครสมาชิกสำเร็จ");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var token = _service.Login(dto);
            return Ok(new { token });
        }
    }

}
