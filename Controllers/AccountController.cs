using Microsoft.AspNetCore.Mvc;

namespace TopUpMVC.Controllers
{
    public class AccountController : Controller
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";

        [HttpGet]
        public IActionResult Login()
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            if (role == "Admin")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                HttpContext.Session.SetString("Role", "Admin");
                TempData["Success"] = "Welcome back, Admin!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Role", "Guest");
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
