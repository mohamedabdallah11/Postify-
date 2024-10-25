using Microsoft.AspNetCore.Mvc;

namespace Profile.Controllers
{
    public class UserRoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
