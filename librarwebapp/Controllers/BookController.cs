using librarwebapp.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace librarwebapp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookExternalService _bookExternalService;
        private readonly ISuscriptionService _suscriptionService;

        public BookController(IBookExternalService bookExternalService,
            ISuscriptionService suscriptionService)
        {
            _bookExternalService = bookExternalService;
            _suscriptionService = suscriptionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Viewer(Guid id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var suscripcionValida = await _suscriptionService.GetValidByUser(Guid.Parse(userId));

            if (suscripcionValida == null)
            {
                return RedirectToAction(nameof(Index), nameof(SuscriptionController));
            }

            var book = await _bookExternalService.GetById(id);
            return View(book);
        }
    }
}
