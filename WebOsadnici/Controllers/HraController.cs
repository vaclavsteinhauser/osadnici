using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebOsadnici.Data;

namespace WebOsadnici.Controllers
{
    public class HraController : Controller
    {
        private readonly ILogger<HraController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Hrac> _userManager;

        public HraController(ILogger<HraController> logger, ApplicationDbContext dbContext, UserManager<Hrac> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.moje = Hra.NactiHry(_dbContext)
                .Where(h => h.hraci.Contains(user)).ToArray();
            ViewBag.nezacate = Hra.NactiHry(_dbContext)
                .Where(h => !h.hraci.Contains(user) && h.hracNaTahu==-1).ToArray();
            return View();
        }
        public async Task<IActionResult> Nova()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Hra h = new Hra(_dbContext.suroviny, _dbContext.stavby);
            _dbContext.Add(h);
            _dbContext.SaveChanges();
            return RedirectToAction("Pripojit",new { id=h.Id });
        }
        public async Task<IActionResult> Pripojit(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Hra h = _dbContext.hry
                .Where(h => h.Id == id)
                .Include(h => h.hraci)
                .Include(h => h.stavy)
                .Single();
            return View(h);
        }

        [HttpPost]
        public async Task<IActionResult> AddStavHrace(IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);
            string barva = form["barva"];
            Guid hraId = Guid.Parse(form["Id"]);
            Hra h = _dbContext.hry
                .Where(h => h.Id == hraId)
                .Include(h => h.hraci)
                .Include(h => h.stavy)
                .Single();
            h.PridejHrace(user,Color.FromName(barva),_dbContext);
            _dbContext.SaveChanges();
            // Přesměrování na akci Prubeh na stejném controlleru
            return RedirectToAction("Prubeh", new {id = hraId});
        }

        public async Task<IActionResult> Prubeh(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Hra h = _dbContext.hry
                .Where(h => h.Id == id)
                .Include(h=>h.mapka)
                .Include (h=>h.hraci)
                .Include(h=>h.stavy)
                .Include(h => h.mapka.rozcesti)
                .Include(h => h.mapka.policka)
                    .ThenInclude(p=>p.surovina)
                .Include(h => h.mapka.cesty)
                .Single();
            if (!h.hraci.Contains(user))
            {
                return RedirectToAction("Index", "Hra");
            }
            return View(h);
        }
    }
}