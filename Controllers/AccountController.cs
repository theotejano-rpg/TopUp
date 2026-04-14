using Microsoft.AspNetCore.Mvc;
using TopUpMVC.Data;
using TopUpMVC.Models;

namespace TopUpMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ─── LOGIN ────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Role") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Check hardcoded admin
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Username", "admin");
                TempData["Success"] = "Welcome back, Admin!";
                return RedirectToAction("Index", "Home");
            }

            // Check registered users in DB
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.FullName);
                TempData["Success"] = $"Welcome back, {user.FullName}!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // ─── REGISTER ─────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Role") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if username already taken
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "That username is already taken. Please choose another.");
                return View(model);
            }

            // Check if email already registered
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with that email already exists.");
                return View(model);
            }

            model.Role = "User";
            model.CreatedAt = DateTime.Now;

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Account created! You can now log in.";
            return RedirectToAction("Login");
        }

        // ─── LOGOUT ───────────────────────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
