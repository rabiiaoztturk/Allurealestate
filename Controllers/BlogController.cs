using Allurerealstate.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Allurerealstate.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var blogs = _context.Blogs.ToList();

            return View(blogs);
        }

        public IActionResult Detail(int id)
        {
            var blogs = _context.Blogs.Find(id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }
    }
}
