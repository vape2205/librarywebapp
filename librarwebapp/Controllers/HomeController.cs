using librarwebapp.Interfaces.Services;
using librarwebapp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace librarwebapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookExternalService _bookExternalService;
        private readonly ISuscriptionService _suscriptionService;

        public HomeController(ILogger<HomeController> logger,
            IBookExternalService bookExternalService,
            ISuscriptionService suscriptionService)
        {
            _logger = logger;
            _bookExternalService = bookExternalService;
            _suscriptionService = suscriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var books = await _bookExternalService.GetAll(pageSize:50);
            var suscripcion = await _suscriptionService.GetValidByUser(Guid.Parse(userId));
            var model = new CatalogoViewModel
            {
                Books = books,
                TieneSuscripcionValida = suscripcion != null
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
