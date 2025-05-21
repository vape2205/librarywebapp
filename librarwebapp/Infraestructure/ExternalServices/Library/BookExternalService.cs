using librarwebapp.Exceptions;
using librarwebapp.Interfaces.Services;
using librarwebapp.Models.ExternalServices.Auth;
using librarwebapp.Models.ExternalServices.Library;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace librarwebapp.Infraestructure.ExternalServices.Library
{
    public class BookExternalService : IBookExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IExternalAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookExternalService(
            HttpClient httpClient,
            IExternalAuthService authService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri(configuration["ExternalServicesSettings:UrlApiLibrary"]);
        }

        public async Task<IEnumerable<BookModel>> FindByAuthor(string queryString,
            int pageNumber = 1, int pageSize = 10)
        {
            await ConfigureClient();

            var url = $"api/book/find-by-title/{queryString}?pageNumber={pageNumber}&pageSize={pageSize}";

            var result = await _httpClient.GetFromJsonAsync<IEnumerable<BookModel>>(url);

            return result;
        }

        public async Task<IEnumerable<BookModel>> FindByTitle(string queryString,
            int pageNumber = 1, int pageSize = 10)
        {
            await ConfigureClient();

            var url = $"api/book/find-by-author/{queryString}?pageNumber={pageNumber}&pageSize={pageSize}";

            var result = await _httpClient.GetFromJsonAsync<IEnumerable<BookModel>>(url);

            return result;
        }

        public async Task<IEnumerable<BookModel>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            await ConfigureClient();

            var result = await _httpClient.GetFromJsonAsync<IEnumerable<BookModel>>(
                    $"api/book?pageNumber={pageNumber}&pageSize={pageSize}");

            return result;
        }

        public async Task<BookModel> GetById(Guid id)
        {
            await ConfigureClient();

            var result = await _httpClient.GetFromJsonAsync<BookModel>(
                    $"api/book/{id}");

            return result;
        }

        private async Task ConfigureClient()
        {
            var token = await GetToken();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> GetToken()
        {
            var accessToken = GetAuthenticationProperty("access_token");
            if (accessToken == null)
            {
                throw new TokenExpiredApiException();
            }
            var expirationDatetime = GetExpirationDateFromToken(accessToken);

            if (DateTime.UtcNow <= expirationDatetime)
            {
                return accessToken;
            }

            var refreshToken = GetAuthenticationProperty("refresh_token");

            var requestRefreshToken = new RefreshTokenRequest
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };
            var resultRefreshToken = await _authService.RefreshToken(requestRefreshToken);

            if (resultRefreshToken.AccessToken == null)
            {
                throw new TokenExpiredApiException();
            }
            SetAuthenticationProperty("access_token", resultRefreshToken.AccessToken);
            SetAuthenticationProperty("refresh_token", resultRefreshToken.RefreshToken);
            return resultRefreshToken.AccessToken;
        }
        private DateTime GetExpirationDateFromToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

        private string GetAuthenticationProperty(string name)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[name];
        }

        private void SetAuthenticationProperty(string name, string value)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(name);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(name, value);
        }
    }
}
