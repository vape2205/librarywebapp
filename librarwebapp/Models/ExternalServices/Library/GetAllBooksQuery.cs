namespace librarwebapp.Models.ExternalServices.Library
{
    public class GetAllBooksQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
