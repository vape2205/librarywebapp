using librarwebapp.Models;
using librarwebapp.Models.ExternalServices.Auth;

namespace librarwebapp.Interfaces.Services
{
    public interface IAppAuthenticationService
    {
        Task Authenticate(LoginDto dto);
        Task<CreateUserResponse> Create(CreateUserRequest dto);
        Task<bool> EnableAuthenticator(EnableAuthenticatorDto model);
        SetupAuthAppDto GenerateTwoFactorInfo(string username);
        bool VerifyTwoFactorAuthentication(TwoFactorLoginDto model);
    }
}
