using Microsoft.AspNetCore.Mvc;
using TopUpMVC.Data;

namespace TopUpMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            ViewBag.Role = role;
            ViewBag.Username = HttpContext.Session.GetString("Username") ?? "Guest";
            ViewBag.TotalTopUps = _context.TopUps.Count();
            ViewBag.PendingCount = _context.TopUps.Count(t => t.Status == "Pending");
            ViewBag.CompletedCount = _context.TopUps.Count(t => t.Status == "Completed");

            // Revenue only for admin
            if (role == "Admin")
                ViewBag.TotalRevenue = _context.TopUps.Sum(t => (decimal?)t.Amount) ?? 0m;

            return View();
        }
    }
}
