using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebOsadnici.Data;
using WebOsadnici.Models;

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
        [Route("Hra/Nova")]
        public async Task<IActionResult> Prubeh()
        {
            Hra h = new Hra();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                h.PridejHrace(user);
            }
            var x = h;
            _dbContext.Add(x);
            _dbContext.SaveChanges();
            return View(h);
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            Hra[] h=Hra.NactiHry(_dbContext)
                .Where(h => h.hraci.Contains(user)).ToArray();
            return View(h);
        }
        [Route("Hra/Prubeh/{id}")]
        public async Task<IActionResult> Prubeh(Guid id)
        {
            Hra h = _dbContext.hry
                .Include(h=>h.mapka)
                .Include (h=>h.hraci)
                .Include(h => h.mapka.rozcesti)
                .Include(h => h.mapka.policka)
                .Include(h => h.mapka.cesty)
                .Where(h=>h.Id==id)
                .FirstOrDefault();
            
            return View(h);
        }
    }
}