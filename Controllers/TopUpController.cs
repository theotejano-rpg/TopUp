using Microsoft.AspNetCore.Mvc;
using TopUpMVC.Data;
using TopUpMVC.Models;

namespace TopUpMVC.Controllers
{
    public class TopUpController : Controller
    {
        private readonly AppDbContext _context;

        public TopUpController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "Guest";
            return View();
        }

        [HttpPost]
        public IActionResult Create(TopUp topup)
        {
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "Guest";

            if (ModelState.IsValid)
            {
                topup.CreatedAt = DateTime.Now;
                topup.Status = "Pending";
                _context.TopUps.Add(topup);
                _context.SaveChanges();
                TempData["Success"] = $"Top-up for {topup.PlayerName} submitted successfully! Transaction ID: #{topup.Id}";
                return RedirectToAction("Index", "Transaction");
            }
            return View(topup);
        }
    }
}
