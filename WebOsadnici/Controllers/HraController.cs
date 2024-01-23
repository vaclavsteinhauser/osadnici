using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebOsadnici.Models;

namespace WebOsadnici.Controllers
{
    public class HraController : Controller
    {
        private readonly ILogger<HraController> _logger;

        public HraController(ILogger<HraController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Hra h=new Hra();
            return View(h);
        }
    }
}