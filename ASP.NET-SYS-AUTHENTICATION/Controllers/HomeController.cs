using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_SYS_AUTHENTICATION.Models;
using ASP.NET_SYS_AUTHENTICATION.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;

        public HomeController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            var model = new DashboardViewModel
            {
                FullName = $"{user.Firstname} {user.Lastname}",
                Email = user.Email,
                LastLogin = user.LastLogin,
                MemberSince = user.CreatedAt
            };

            return View(model);
        }
    }
}