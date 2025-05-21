using librarwebapp.Interfaces.Services;
using librarwebapp.Models.ExternalServices.Suscriptions;

namespace librarwebapp.Services
{
    public class SuscriptionService : ISuscriptionService
    {
        private readonly ISuscriptionExternalService _suscriptionExternalService;

        public SuscriptionService(ISuscriptionExternalService suscriptionExternalService)
        {
            _suscriptionExternalService = suscriptionExternalService;
        }

        public async Task<SuscripcionDTO> GetValidByUser(Guid userId)
        {
            var fechaActual = DateTime.UtcNow;
            var suscriptions = await _suscriptionExternalService.FindByUserId(userId);
            var result = suscriptions.FirstOrDefault(x => (x.Estado == EstadoSuscripcion.Activo
                                || x.Estado == EstadoSuscripcion.Cancelado)
                                && fechaActual >= x.FechaInicio
                                && fechaActual <= x.FechaFin);
            return result;
        }
    }
}
