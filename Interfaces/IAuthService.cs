using api_pd.DTOs.Auth;

namespace api_pd.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        string Login(LoginDto dto);
    }

}
