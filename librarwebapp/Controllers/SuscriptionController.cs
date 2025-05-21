using librarwebapp.Interfaces.Services;
using librarwebapp.Models.ExternalServices.Suscriptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace librarwebapp.Controllers
{
    public class SuscriptionController : Controller
    {
        private readonly ISuscriptionExternalService _suscriptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SuscriptionController(ISuscriptionExternalService suscriptionService,
            IHttpContextAccessor httpContextAccessor)
        {
            _suscriptionService = suscriptionService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var suscripciones = await _suscriptionService.FindByUserId(Guid.Parse(userId));
            return View(suscripciones);
        }

        public IActionResult Crear()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new CrearSuscripcionDTO
            {
                UserId = Guid.Parse(userId),
                PrecioPorMes = 50.00M
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearSuscripcionDTO model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var result = await _suscriptionService.Crear(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activar(Guid id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _suscriptionService.Activar(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _suscriptionService.Cancelar(id);

            return RedirectToAction(nameof(Index)); ;
        }
    }
}
