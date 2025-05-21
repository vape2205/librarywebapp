using librarwebapp.Exceptions;
using librarwebapp.Interfaces.Services;
using librarwebapp.Models.ExternalServices.Auth;
using librarwebapp.Models.ExternalServices.Suscriptions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace librarwebapp.Infraestructure.ExternalServices.Suscriptions
{
    public class SuscriptionExternalService : ISuscriptionExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExternalAuthService _authService;

        public SuscriptionExternalService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IExternalAuthService authService,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;

            _httpClient.BaseAddress = new Uri(configuration["ExternalServicesSettings:UrlApiSuscriptions"]);
        }

        public async Task<SuscripcionDTO> GetById(Guid id)
        {
            await ConfigureClient();

            var result = await _httpClient.GetFromJsonAsync<SuscripcionDTO>(
                    $"api/suscripciones/{id}");
            return result;
        }

        public async Task<IEnumerable<SuscripcionDTO>> FindByUserId(Guid userId)
        {
            await ConfigureClient();

            var result = await _httpClient.GetFromJsonAsync<IEnumerable<SuscripcionDTO>>(
                    $"api/suscripciones/user/{userId}");
            return result;
        }

        public async Task Activar(Guid id)
        {
            await ConfigureClient();

            var result = await _httpClient.PostAsync(
                    $"api/suscripciones/{id}/activar", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task Cancelar(Guid id)
        {
            await ConfigureClient();

            var result = await _httpClient.PostAsync(
                    $"api/suscripciones/{id}/cancelar", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task<SuscripcionDTO> Crear(CrearSuscripcionDTO dto)
        {
            await ConfigureClient();

            var httpResult = await _httpClient.PostAsJsonAsync(
                    $"api/suscripciones", dto);
            httpResult.EnsureSuccessStatusCode();
            SuscripcionDTO response = await httpResult.Content.ReadFromJsonAsync<SuscripcionDTO>();
            return response;
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
