using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Models;

namespace Task4.Controllers
{
    public class AccountController: Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.IsBlocked || !VerifyPassword(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid credentials or user is blocked.");
                return View();
            }

            user.LastLoginTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Items.ContainsKey("BlockedMessage"))
            {
                ViewBag.BlockedMessage = HttpContext.Items["BlockedMessage"];
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email)) {
                ViewData["ErrorMessage"] = "Пользователь с таким email уже существует.";
                return View();
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password), // Реализуйте метод хэширования пароля
                IsBlocked = false,
                RegistrationTime = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            return password; 
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return password == hashedPassword;
        }
    }
}
