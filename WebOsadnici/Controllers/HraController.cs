using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebOsadnici.Data;
using WebOsadnici.Models.HerniTridy;

namespace WebOsadnici.Controllers
{
    public class HraController : Controller
    {
        private readonly ILogger<HraController> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly UserManager<Hrac> _userManager;

        public HraController(ILogger<HraController> logger, IDbContextFactory<ApplicationDbContext> dbContextFactory, UserManager<Hrac> userManager)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: Hra
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Hra/Nova
        public async Task<IActionResult> Nova()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hra = new Hra();
            hra._dbContext = _dbContextFactory.CreateDbContext();
            await hra.Inicializace();

            return RedirectToAction("Pripojit", new { id = hra.Id });
        }

        // GET: Hra/Pripojit?id={id}
        public async Task<IActionResult> Pripojit(Guid id)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hra = await Hra.NactiHru(id, _dbContextFactory.CreateDbContext());
            if (hra == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.hra = hra;
            return View();
        }

        // POST: Hra/AddStavHrace
        [HttpPost]
        public async Task<IActionResult> AddStavHrace(IFormCollection form)
        {
            var user = await GetUserAsync();
            var barva = form["barva"];
            var hraId = Guid.Parse(form["Id"]);
            var hra = await Hra.NactiHru(hraId, _dbContextFactory.CreateDbContext());
            if (hra == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (hra.hraci.Contains(user))
            {
                return RedirectToAction("Prubeh", new { id = hraId });
            }

            await hra.PridejHrace(user, Color.FromName(barva));

            return RedirectToAction("Prubeh", new { id = hraId });
        }

        // GET: Hra/Prubeh?id={id}
        public async Task<IActionResult> Prubeh(Guid id)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hra = await Hra.NactiHru(id, _dbContextFactory.CreateDbContext());
            if (hra == null || !hra.hraci.Contains(user))
            {
                return RedirectToAction("Index", "Hra");
            }

            ViewBag.hra = hra;
            ViewBag.hrac = user;
            ViewBag.stavby = hra.mapka.Stavby;
            return View();
        }

        // Helper method to get the current user asynchronously
        private async Task<Hrac> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
