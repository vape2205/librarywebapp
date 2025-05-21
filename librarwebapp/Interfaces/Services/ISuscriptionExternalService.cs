using librarwebapp.Models.ExternalServices.Suscriptions;

namespace librarwebapp.Interfaces.Services
{
    public interface ISuscriptionExternalService
    {
        Task<SuscripcionDTO> GetById(Guid id);
        Task<IEnumerable<SuscripcionDTO>> FindByUserId(Guid userId);
        Task<SuscripcionDTO> Crear(CrearSuscripcionDTO dto);
        Task Activar(Guid id);
        Task Cancelar(Guid id);
    }
}
