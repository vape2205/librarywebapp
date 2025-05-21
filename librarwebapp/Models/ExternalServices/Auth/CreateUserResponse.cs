namespace librarwebapp.Models.ExternalServices.Auth
{
    public class CreateUserResponse
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
