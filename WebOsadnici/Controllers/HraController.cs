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
        public IActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }
        
        public async Task<IActionResult> Nova()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Hra h = new Hra();
            await h.Inicializace(_dbContext);
            Hra.NactiHru(h);
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
            Hra h = Hra.NactiHru(id, _dbContext);
            ViewBag.hra = h;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStavHrace(IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);
            string barva = form["barva"];
            Guid hraId = Guid.Parse(form["Id"]);
            Hra h = Hra.NactiHru(hraId, _dbContext);
            if (h == null)
            {
                return RedirectToAction("Index", "Home");
            }
            await h.PridejHrace(user,Color.FromName(barva),_dbContext);

            // Přesměrování na akci Prubeh na stejném controlleru
            return RedirectToAction("Prubeh", new {id = hraId});
        }

        public async Task<IActionResult> Prubeh(Guid id)
        {
            Hrac user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Hra h = Hra.NactiHru(id, _dbContext);
            if (h==null || !h.hraci.Contains(user))
            {
                return RedirectToAction("Index", "Hra");
            }
            ViewBag.hra = h;
            ViewBag.hrac = user;
            ViewBag.stavby = _dbContext.stavby.ToArray();
            return View();
        }
    }
}