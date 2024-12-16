using Allurerealstate.Data;
using Allurerealstate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Allurerealstate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var favoriteProperties = _context.Properties
             .Where(p => p.Favorite)
             .ToList();
            return View(favoriteProperties);
        }
    }
}
