using librarwebapp.Models.ExternalServices.Library;

namespace librarwebapp.Interfaces.Services
{
    public interface IBookExternalService
    {
        Task<BookModel> GetById(Guid id);
        Task<IEnumerable<BookModel>> GetAll(int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<BookModel>> FindByTitle(string queryString, int pageNumber = 1, int pageSize = 10);
        Task<IEnumerable<BookModel>> FindByAuthor(string queryString, int pageNumber = 1, int pageSize = 10);
    }
}
