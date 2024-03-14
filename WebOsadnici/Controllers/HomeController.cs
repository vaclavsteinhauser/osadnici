using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebOsadnici.Data;
using WebOsadnici.Models;
using WebOsadnici.Models.HerniTridy;

namespace WebOsadnici.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Hrac> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<Hrac> userManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                ViewBag.moje = await _dbContext.hry
                    .Where(h => h.stavy.Any(s => s.hrac.Id == user.Id) && h.stavHry != StavHry.Skoncila)
                    .ToArrayAsync();

                ViewBag.nezacate = await _dbContext.hry
                    .Where(h => h.stavy.All(s => s.hrac.Id != user.Id) && h.stavHry == StavHry.Nezacala)
                    .ToArrayAsync();
                ViewBag.ukoncene = await _dbContext.hry
                    .Where(h => h.stavHry == StavHry.Skoncila && h.stavy.Any(s => s.hrac.Id == user.Id))
                    .ToArrayAsync();
                ViewBag.hrac = user;
            }
            return View();
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Helper method to get the current user asynchronously
        private async Task<Hrac> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
