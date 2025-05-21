namespace librarwebapp.Models.ExternalServices.Auth
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
        public string Message { get; internal set; }
        public string Code { get; internal set; }
    }
}
