using Microsoft.AspNetCore.Mvc;
using TopUpMVC.Data;
using TopUpMVC.Models;

namespace TopUpMVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string game, string status)
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            ViewBag.Role = role;
            ViewBag.Search = search;
            ViewBag.Game = game;
            ViewBag.Status = status;

            var topups = _context.TopUps.ToList();

            if (!string.IsNullOrEmpty(search))
                topups = topups.Where(t =>
                    t.PlayerName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.PlayerId.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.Email.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (!string.IsNullOrEmpty(game))
                topups = topups.Where(t => t.Game == game).ToList();

            if (!string.IsNullOrEmpty(status))
                topups = topups.Where(t => t.Status == status).ToList();

            return View(topups);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            if (role != "Admin")
                return RedirectToAction("Index");

            var topup = _context.TopUps.Find(id);
            if (topup != null)
            {
                topup.Status = status;
                _context.SaveChanges();
                TempData["Success"] = $"Transaction #{id} marked as {status}.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            if (role != "Admin")
                return RedirectToAction("Index");

            var topup = _context.TopUps.Find(id);
            if (topup != null)
            {
                _context.TopUps.Remove(topup);
                _context.SaveChanges();
                TempData["Success"] = $"Transaction #{id} deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}
