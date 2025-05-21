namespace librarwebapp.Models
{
    public class EnableAuthenticatorDto
    {
        public string Username { get; set; }
        public string SecretKey { get; set; }
        public string AuthenticatorUri { get; set; }
        public string Code { get; set; }
    }
}
