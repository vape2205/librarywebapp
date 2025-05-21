namespace librarwebapp.Models.ExternalServices.Library
{
    public class BookModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Authors { get; set; }
        public string Isdn { get; set; }
        public string Base64File { get; set; }

        public string UrlDocument { get; set; }
    }
}
