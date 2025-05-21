namespace librarwebapp.Models.ExternalServices.Library
{
    public class FindBooksByTitleQuery
    {
        public string QueryString { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
