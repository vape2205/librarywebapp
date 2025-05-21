using librarwebapp.Models.ExternalServices.Library;

namespace librarwebapp.Models
{
    public class CatalogoViewModel
    {
        public bool TieneSuscripcionValida { get; set; }
        public IEnumerable<BookModel> Books { get; set; }
    }
}
