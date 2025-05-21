using librarwebapp.Interfaces.Services;
using librarwebapp.Models;
using librarwebapp.Models.ExternalServices.Auth;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace librarwebapp.Services
{
    public class AuthenticationService : IAppAuthenticationService
    {
        private readonly IExternalAuthService _authService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationService(IExternalAuthService authService,
            IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
        }

        public async Task Authenticate(LoginDto dto)
        {
            var response = await _authService.Login(dto);
            if (response == null)
            {
                return;
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(response.IdToken);
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value ?? string.Empty;

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dto.Username),
                    new Claim(ClaimTypes.NameIdentifier, userId)
                };
            ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            var properties = new AuthenticationProperties();
            properties.IsPersistent = true;
            properties.Items["access_token"] = response.AccessToken;
            properties.Items["refresh_token"] = response.RefreshToken;
            properties.Items["id_token"] = response.IdToken;

            await _contextAccessor.HttpContext.SignInAsync(principal, properties);
            _contextAccessor.HttpContext.Response.Cookies.Append("access_token", response.AccessToken);
            _contextAccessor.HttpContext.Response.Cookies.Append("refresh_token", response.RefreshToken);
            _contextAccessor.HttpContext.Response.Cookies.Append("id_token", response.IdToken);
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest model)
        {
            return await _authService.Create(model);
        }

        public async Task<bool> EnableAuthenticator(EnableAuthenticatorDto model)
        {
            //try
            //{
            //    var totp = new Totp(Base32Encoding.ToBytes(model.SecretKey));
            //    var verified = totp.VerifyTotp(model.Code, out _, new VerificationWindow(2, 2)); // Adjust window size as needed
            //    if (!verified)
            //    {
            //        return false;
            //    }

            //    var user = _userService.FindByUsername(model.Username);
            //    if (user == null)
            //    {
            //        return false;
            //    }
            //    user.SecretKey = model.SecretKey;
            //    user.IsTwoFactorEnabled = true;
            //    await _userService.Update(user);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
            return false;

        }

        public SetupAuthAppDto GenerateTwoFactorInfo(string username)
        {
            string secretKey = GenerateSecretKey();

            var encodedUsername = Uri.EscapeDataString(username);
            var qrCodeUrl = $"otpauth://totp/{encodedUsername}?secret={secretKey}&issuer=TwoFactorAuthApp&digits=6";

            return new SetupAuthAppDto
            {
                SecretKey = secretKey,
                AuthenticatorUri = qrCodeUrl
            };
        }

        public bool VerifyTwoFactorAuthentication(TwoFactorLoginDto model)
        {
            //try
            //{
            //    var user = _userService.FindByUsername(model.Username);

            //    if (user != null && !string.IsNullOrEmpty(user.SecretKey) && !string.IsNullOrEmpty(model.Code))
            //    {
            //        var totp = new Totp(Base32Encoding.ToBytes(user.SecretKey));
            //        var verifed = totp.VerifyTotp(model.Code, out _, new VerificationWindow(2, 2)); // Adjust window size as needed
            //        return verifed;
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
            return false;
        }

        private static string GenerateSecretKey()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var random = new Random();

            // Generate a random secret key
            var secretKeyBuilder = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
            {
                secretKeyBuilder.Append(validChars[random.Next(validChars.Length)]);
            }

            var secretKey = secretKeyBuilder.ToString();
            return secretKey;
        }
    }
}