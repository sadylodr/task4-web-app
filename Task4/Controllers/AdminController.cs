using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Models;

namespace Task4.Controllers
{
    [Authorize]
    public class AdminController: Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            IQueryable<User> usersQuery = _context.Users;

            usersQuery = usersQuery.OrderByDescending(u => u.LastLoginTime ?? DateTime.MinValue);

            var users = await usersQuery.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Block(List<int> ids)
        {
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            foreach (var user in users) user.IsBlocked = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(List<int> ids)
        {
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            foreach (var user in users) user.IsBlocked = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            var users = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
