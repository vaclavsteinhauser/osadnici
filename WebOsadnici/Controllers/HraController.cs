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

            _dbContext.hry.Add(h);
            _dbContext.SaveChanges();
            return View(h);
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            Hra[] h=_dbContext.hry.Include(h=>h.hraci).Include(h=>h.mapka).Where(h => h.hraci.Contains(user)).ToArray();
            return View(h);
        }
        [Route("Hra/Prubeh/{id}")]
        public async Task<IActionResult> Prubeh(Guid id)
        {
            Hra h = _dbContext.hry.Where(h=>h.Id==id).FirstOrDefault();
            
            return View(h);
        }
    }
}