using librarwebapp.Interfaces.Services;
using librarwebapp.Models;
using librarwebapp.Models.ExternalServices.Auth;
using System.Security.Claims;

namespace librarwebapp.Infraestructure.ExternalServices.Auth
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExternalAuthService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri(configuration["ExternalServicesSettings:UrlApiAuth"]);
        }

        public async Task<LoginResponse> Login(LoginDto dto)
        {
            await ConfigureClient();

            var request = new LoginRequest
            {
                Email = dto.Username,
                Password = dto.Password
            };

            var httpResult = await _httpClient.PostAsJsonAsync(
                    "api/account/login", request);

            LoginResponse response = await httpResult.Content.ReadFromJsonAsync<LoginResponse>();
            return response;
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest request)
        {
            await ConfigureClient();

            var httpResult = await _httpClient.PostAsJsonAsync(
                    "api/account/createuser", request);

            var response = await httpResult.Content.ReadFromJsonAsync<CreateUserResponse>();
            return response;
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            await ConfigureClient();

            var httpResult = await _httpClient.PostAsJsonAsync(
                    "api/account/refreshtoken", request);

            httpResult.EnsureSuccessStatusCode();

            RefreshTokenResponse response = await httpResult.Content.ReadFromJsonAsync<RefreshTokenResponse>();
            return response;
        }

        public async Task<SignOutResponse> SignOut()
        {
            await ConfigureClient();

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var request = new SignOutRequest { UserId = Guid.Parse(userId) };

            var httpResult = await _httpClient.PostAsJsonAsync(
                    "api/account/signout", request);

            SignOutResponse response = await httpResult.Content.ReadFromJsonAsync<SignOutResponse>();
            return response;
        }

        private Task ConfigureClient()
        {
            return Task.CompletedTask;
        } 
    }
}
