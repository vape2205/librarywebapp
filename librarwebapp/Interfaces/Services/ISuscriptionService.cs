using librarwebapp.Models.ExternalServices.Suscriptions;

namespace librarwebapp.Interfaces.Services
{
    public interface ISuscriptionService
    {
        Task<SuscripcionDTO> GetValidByUser(Guid userId);
    }
}
