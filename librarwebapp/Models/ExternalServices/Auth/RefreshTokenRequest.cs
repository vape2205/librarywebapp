namespace librarwebapp.Models.ExternalServices.Auth
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
