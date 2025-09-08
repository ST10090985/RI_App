using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Constructor receives a logger instance through dependency injection
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Default landing page
        public IActionResult Index()
        {
            return View();
        }

        // Privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Error page - shown when something goes wrong
        // Uses ResponseCache so errors are never cached
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Passes the current request ID to the view for easier debugging
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
