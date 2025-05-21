namespace librarwebapp.Exceptions
{
    public class TokenExpiredApiException : Exception
    {
        public TokenExpiredApiException()
        {
        }

        public TokenExpiredApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
