using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class TokenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
