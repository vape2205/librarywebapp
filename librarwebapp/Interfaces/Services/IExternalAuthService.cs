using librarwebapp.Models;
using librarwebapp.Models.ExternalServices.Auth;

namespace librarwebapp.Interfaces.Services
{
    public interface IExternalAuthService
    {
        Task<LoginResponse> Login(LoginDto dto);
        Task<CreateUserResponse> Create(CreateUserRequest dto);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
        Task<SignOutResponse> SignOut();
    }
}
