using Microsoft.AspNetCore.Mvc;

namespace ReservasiAPI.Data
{
    public class ApplicationDbContext : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
