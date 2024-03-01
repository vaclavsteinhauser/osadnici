using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebOsadnici.Data;
using WebOsadnici.Models;

namespace WebOsadnici.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Hrac> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<Hrac> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.moje = Hra.NactiNahledyHer(_dbContext)
                    .Where(h => h.hraci.Contains(user)).ToArray();
                ViewBag.nezacate = Hra.NactiNahledyHer(_dbContext)
                    .Where(h => !h.hraci.Contains(user) && h.hracNaTahu == -1).ToArray();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}