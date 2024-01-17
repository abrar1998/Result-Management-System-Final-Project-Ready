using Microsoft.AspNetCore.Mvc;

namespace RMS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashBoard()
        {
            return View();
        }
    }
}
