namespace librarwebapp.Models.ExternalServices.Auth
{
    public class LoginResponse
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; internal set; }
        public string Code { get; internal set; }
    }
}
